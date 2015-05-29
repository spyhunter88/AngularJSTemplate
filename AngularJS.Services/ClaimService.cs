using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AngularJS.Entities.Models;
using AngularJS.Entities;
using AngularJS.Services.DTO;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System.IO;
using Omu.ValueInjecter;
using AngularJS.Services.InjectConfig;
using AngularJS.Services.Utility;

namespace AngularJS.Service
{
    public interface IClaimService
    {
        List<ClaimLiteDTO> SearchClaim(String searchText, string orderBy, bool descending, int pageSize, int page, out int totalCount);

        ClaimLiteDTO GetClaimLite(int id);

        Task<ClaimDTO> GetClaimAsync(int id, List<ObjectConfig> excList);

        Task<int> PostClaim(ClaimDTO claim, string uploadPath);

        Task<ClaimDTO> PutClaim(ClaimDTO claim, string uploadPath, int userID, string action);
    }

    public class ClaimService : IClaimService
    {
        public ClaimService(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public List<ClaimLiteDTO> SearchClaim(String searchText, string orderBy, bool descending, int pageSize, int page, out int totalCount)
        {
            if (orderBy == "") orderBy = "CreateTime";

            // Load
            List<Claim> _claims = new List<Claim>(_unitOfWorkAsync.RepositoryAsync<Claim>()
                                    .Query(x => x.FtgProgramCode.Contains(searchText) || x.ProgramName.Contains(searchText))
                                    .OrderBy(q => q.OrderBy(orderBy, descending))
                                    .SelectPage(page, pageSize, out totalCount)
            );

            Random rand = new Random(100);

            List<ClaimLiteDTO> result = AutoMapper.Mapper.Map<List<Claim>, List<ClaimLiteDTO>>(_claims);

            foreach(ClaimLiteDTO _claim in result)
            {
                var _status = _unitOfWorkAsync.Repository<ClaimStatus>().Query(x => x.Code == _claim.StatusID)
                    .Select(s => new { s.StatusName, s.Phase })
                    .FirstOrDefault();

                _claim.Status = _status.StatusName;
                _claim.Phase = _status.Phase;

                var _userInfo = _unitOfWorkAsync.Repository<User>().Query(x => x.Id == _claim.CreateBy)
                    .Select(u => u.UserName)
                    .FirstOrDefault();
                _claim.CreateUser = _userInfo;

                var _editInfo = _unitOfWorkAsync.Repository<User>().Query(x => x.Id == _claim.LastEditBy)
                    .Select(u => u.UserName)
                    .FirstOrDefault();
                _claim.LastEditUser = _editInfo;

                var _totalPayment = _unitOfWorkAsync.Repository<Payment>().Queryable()
                    .Sum(x => x.VendorPayment);
                _claim.RemainPayment = (_claim.VendorConfirmAmount ?? 0) - (_totalPayment ?? 0);

                var _totalPaymentVND = _unitOfWorkAsync.Repository<Payment>().Queryable()
                    .Sum(x => x.VendorPayment * x.ExchangeRate);
                var _totalAllocation = _unitOfWorkAsync.Repository<Allocation>().Queryable()
                    .Sum(x => x.AllocateAmount);

                _claim.RemainAllocation = (_claim.RemainPayment * (_claim.ExchangeRate ?? 0)) + 
                                (_totalPaymentVND ?? 0) - (_totalAllocation ?? 0);
            }

            return result;
        }

        public ClaimLiteDTO GetClaimLite(int id)
        {
            var claimLite = new ClaimLiteDTO();

            var _claim = _unitOfWorkAsync.Repository<Claim>().Query(c => c.ClaimID == id)
                .Select()
                .FirstOrDefault();

            if (_claim != null)
            {
                claimLite = AutoMapper.Mapper.Map<Claim, ClaimLiteDTO>(_claim);

                var _status = _unitOfWorkAsync.Repository<ClaimStatus>().Query(x => x.Code == _claim.StatusID)
                    .Select(s => new { s.StatusName, s.Phase })
                    .FirstOrDefault();

                claimLite.Status = _status.StatusName;
                claimLite.Phase = _status.Phase;
            }

            return claimLite;
        }

        public async Task<ClaimDTO> GetClaimAsync(int id, List<ObjectConfig> excList)
        {
            // Get Claim values and properties base on objectconfig above
            var query = _unitOfWorkAsync.RepositoryAsync<Claim>().Query(x => x.ClaimID == id);
            if (excList.Where(x => String.Equals(x.ObjectField, "CheckPoints", StringComparison.OrdinalIgnoreCase)).Count() == 0) query.Include(x => x.CheckPoints);
            if (excList.Where(x => String.Equals(x.ObjectField, "Requirements", StringComparison.OrdinalIgnoreCase)).Count() == 0) query.Include(x => x.Requirements);
            if (excList.Where(x => String.Equals(x.ObjectField, "Payments", StringComparison.OrdinalIgnoreCase)).Count() == 0) query.Include(x => x.Payments);
            if (excList.Where(x => String.Equals(x.ObjectField, "Allocations", StringComparison.OrdinalIgnoreCase)).Count() == 0) query.Include(x => x.Allocations);

            var _claim = (await query.SelectAsync()).FirstOrDefault();

            // Transform Claim to ClaimDTO and copy other list
            ClaimDTO claim = AutoMapper.Mapper.Map<Claim, ClaimDTO>(_claim);
            claim.CheckPoints = AutoMapper.Mapper.Map<ICollection<CheckPoint>, List<CheckPointDTO>>(_claim.CheckPoints);
            claim.Requirements = AutoMapper.Mapper.Map<ICollection<Requirement>, List<RequirementDTO>>(_claim.Requirements);
            claim.Payments = AutoMapper.Mapper.Map<ICollection<Payment>, List<PaymentDTO>>(_claim.Payments);
            claim.Allocations = AutoMapper.Mapper.Map<ICollection<Allocation>, List<AllocationDTO>>(_claim.Allocations);

            // Load status
            var _status = _unitOfWorkAsync.Repository<ClaimStatus>().Query(x => x.Code == claim.StatusID)
             .Select(s => new { s.StatusName, s.Phase })
             .FirstOrDefault();
            claim.Status = _status.StatusName;
            claim.Phase = _status.Phase;

            // Get UserName
            var _user = _unitOfWorkAsync.Repository<User>().Query(x => x.Id == _claim.CreateBy)
                .Select(x => x.UserName)
                .FirstOrDefault();
            claim.CreateUser = _user;

            // Load Allocation info
            foreach (AllocationDTO aloc in claim.Allocations)
            {
                aloc.InvoiceCode = claim.Payments.Where(x => x.PaymentID == aloc.PaymentID)
                    .Select(x => x.InvoiceCode)
                    .FirstOrDefault();
            }

            return claim;
        }

        /// <summary>
        /// Create new Claim, include CheckPoints, Requirements and Documents' path
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        public async Task<int> PostClaim(ClaimDTO claim, string uploadPath)
        {
            Claim _claim = AutoMapper.Mapper.Map<ClaimDTO, Claim>(claim);
            _claim.CheckPoints = AutoMapper.Mapper.Map<List<CheckPointDTO>, ICollection<CheckPoint>>(claim.CheckPoints);
            _claim.Requirements = AutoMapper.Mapper.Map<List<RequirementDTO>, ICollection<Requirement>>(claim.Requirements);
            // _claim.do

            _claim.StatusID = 1;

            _claim.ObjectState = ObjectState.Added;
            foreach (CheckPoint cp in _claim.CheckPoints)
            {
                cp.ObjectState = ObjectState.Added;
            }
            foreach (Requirement req in _claim.Requirements)
            {
                req.ObjectState = ObjectState.Added;
            }

            _unitOfWorkAsync.RepositoryAsync<Claim>().Insert(_claim);
            int record = _unitOfWorkAsync.SaveChanges();
            int newId = _claim.ClaimID;

            // Rename and move documents to Claim's folder
            string source = uploadPath + "/Temps", dest = uploadPath + "/Claim/" + newId;
            foreach (DocumentDTO doc in claim.Documents)
            {
                Directory.CreateDirectory(dest);
                File.Move(source + "/" + doc.TempName, dest + "/" + doc.FileName);
                Document _doc = AutoMapper.Mapper.Map<DocumentDTO, Document>(doc);
                _doc.ReferenceID = (short) newId;
                _doc.ReferenceName = "Claim";
                _doc.ObjectState = ObjectState.Added;
                _unitOfWorkAsync.Repository<Document>().Insert(_doc);
            }

            record = await _unitOfWorkAsync.SaveChangesAsync();

            return newId;
        }

        /// <summary>
        /// Use to update/submit/approve/deny/failure/done ... claim from workflow
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="uploadPath"></param>
        /// <param name="action">action for this claim</param>
        /// <returns></returns>
        public async Task<ClaimDTO> PutClaim(ClaimDTO claim, string uploadPath, int userID, string action)
        {
            Claim _claim = AutoMapper.Mapper.Map<ClaimDTO, Claim>(claim);
            _claim.CheckPoints = AutoMapper.Mapper.Map<List<CheckPointDTO>, ICollection<CheckPoint>>(claim.CheckPoints);
            _claim.Requirements = AutoMapper.Mapper.Map<List<RequirementDTO>, ICollection<Requirement>>(claim.Requirements);
            _claim.Payments = AutoMapper.Mapper.Map<List<PaymentDTO>, ICollection<Payment>>(claim.Payments);
            _claim.Allocations = AutoMapper.Mapper.Map<List<AllocationDTO>, ICollection<Allocation>>(claim.Allocations);

            // Load new string[] from objectConfig
            var phase = _unitOfWorkAsync.Repository<ClaimStatus>().Query(cs => cs.Code == claim.StatusID).Select(s => s.Phase).FirstOrDefault();
            string[] objectConfig = PolicyUtil.GetObjectConfig(_unitOfWorkAsync, userID, "Claim", phase, userID == claim.CreateBy)
                                    .AsQueryable()
                                    .Where(x => x.FieldProperty == "disabled")
                                    .Select(x => x.ObjectField)
                                    .ToArray();
            ClaimExcInjection _cei = new ClaimExcInjection(objectConfig);

            // Load current claim and inject
            var targets = await _unitOfWorkAsync.RepositoryAsync<Claim>().Query(x => x.ClaimID == _claim.ClaimID).SelectAsync();
            var target = targets.FirstOrDefault();
            target.InjectFrom(_cei, _claim);

            // manual copy list
            if (!objectConfig.Contains("checkpoints"))
            {
                target.CheckPoints = _claim.CheckPoints;
            }
            if (!objectConfig.Contains("requirements"))
            {
                target.Requirements = _claim.Requirements;
            }
            if (!objectConfig.Contains("payments"))
            {
                target.Payments = _claim.Payments;
            }
            if (!objectConfig.Contains("allocations"))
            {
                target.Allocations = _claim.Allocations;
            }


            // Check for other value, change status
            switch (action)
            {
                case "Save":
                    // Dont change anything, just SAVE depend on status
                    break;
                case "Submit":
                    {
                        // Case: DRAFT -> PREPARE, RUNNING or ENDING
                        if (target.StatusID == 1) 
                        {
                            DateTime now = new DateTime();
                            if (target.StartDate > now) target.StatusID = 10;       // PREPARE RUNNING
                            else if (target.EndDate > now) target.StatusID = 11;    // RUNNING
                            else target.StatusID = 12;                              // ENDING
                        }

                        // Case: ENDING -> WAITING APPROVE or SUBMITTED CLAIM
                        if (target.StatusID == 12) target.StatusID = ((int)target.PrePaid == 1) ? (short)16 : (short)13;

                        // Case: PREPARE CLAIM -> SUBMITTED CLAIM, SUBMITTED CLAIM -> SUBMITED CLAIM or DONE!
                        if (target.StatusID == 15)
                        {
                            if (target.VendorConfirmDate != null && (target.VendorConfirmAmount != 0)
                                && target.SubmitClaimDate != null)
                                target.StatusID = 15;
                            else
                                target.StatusID = 14;
                        }
                        if (target.StatusID == 16) target.StatusID = 17;
                        break;
                    }
                case "Approve":
                    // Case: WAITING APPROVE -> PREPARE CLAIM
                    if (target.StatusID == 13) target.StatusID = 14;
                    break;
                case "Deny":
                    // Case: WAITING APPROVE -> ENDING
                    if (target.StatusID == 13) target.StatusID = 12;
                    break;
            }


            _unitOfWorkAsync.RepositoryAsync<Claim>().Update(target);
            var claimId = _unitOfWorkAsync.SaveChangesAsync();

            return AutoMapper.Mapper.Map<Claim, ClaimDTO>(target);
        }
    }
}
