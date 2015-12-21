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
using AngularJS.Services.General;

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

           //  Random rand = new Random(100);

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
            if (excList.Where(x => String.Equals(x.ObjectField, "Documents", StringComparison.OrdinalIgnoreCase)).Count() == 0) query.Include(x => x.ClaimDocuments);

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

            foreach (DocumentDTO doc in claim.Documents)
            {
                DocumentDTO docDTO = AutoMapper.Mapper.Map<DocumentDTO>(doc);
                docDTO.UploadUser = _unitOfWorkAsync.Repository<User>().Query(x => x.Id == doc.UploadBy)
                    .Select(x => x.UserName)
                    .FirstOrDefault();

                // claim.Documents.Add(docDTO);
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

            _claim.StatusID = (short)Status.CLAIM_DRAFT;

            _claim.ObjectState = ObjectState.Added;
            foreach (CheckPoint cp in _claim.CheckPoints)
            {
                cp.ObjectState = ObjectState.Added;
            }
            foreach (Requirement req in _claim.Requirements)
            {
                req.ObjectState = ObjectState.Added;
            }
            foreach (ClaimDocument doc in _claim.ClaimDocuments)
            {
                doc.ObjectState = ObjectState.Added;
            }

            _unitOfWorkAsync.RepositoryAsync<Claim>().InsertOrUpdateGraph(_claim);
            int record = _unitOfWorkAsync.SaveChanges();
            int newId = _claim.ClaimID;

            // Rename and move documents to Claim's folder
            string source = uploadPath + "/Temps", dest = uploadPath + "/Claims/" + newId;
            Directory.CreateDirectory(dest);
            foreach (DocumentDTO doc in claim.Documents)
            {
                File.Move(source + "/" + doc.TempName, dest + "/" + doc.FileName);
            }

            // record = await _unitOfWorkAsync.SaveChangesAsync();

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

            // Load current claim
            var target = _unitOfWorkAsync.Repository<Claim>().Query(x => x.ClaimID == _claim.ClaimID).Select().FirstOrDefault();

            // Load new string[] from objectConfig
            var phase = _unitOfWorkAsync.Repository<ClaimStatus>().Query(cs => cs.Code == target.StatusID).Select(s => s.Phase).FirstOrDefault();
            string[] objectConfig = PolicyUtil.GetObjectConfig(_unitOfWorkAsync, userID, "Claim", phase, userID == target.CreateBy)
                                    .AsQueryable()
                                    .Where(x => x.FieldProperty == "disabled")
                                    .Select(x => x.ObjectField)
                                    .ToArray();
            ClaimExcInjection _cei = new ClaimExcInjection(objectConfig);

            // Reload Claim with full required information to inject/update
            var query = _unitOfWorkAsync.Repository<Claim>().Query(x => x.ClaimID == _claim.ClaimID);
            if (!objectConfig.Contains("checkpoints")) query.Include(x => x.CheckPoints);
            if (!objectConfig.Contains("requirements")) query.Include(x => x.Requirements);
            if (!objectConfig.Contains("payments")) query.Include(x => x.Payments);
            if (!objectConfig.Contains("allocations")) query.Include(x => x.Allocations);
            if (!objectConfig.Contains("documents")) query.Include(x => x.ClaimDocuments);
            target = query.Select().FirstOrDefault();

            // Inject from DTO to current
            target.InjectFrom(_cei, _claim);

            // Create Value injecter for inner class
            List<string> cpObjCfg = objectConfig.Where(x => x.Contains("claim.")).Select(x => x.RemovePrefix("claim.")).ToList().ToList();
            cpObjCfg.Add("claim");
            ClaimExcInjection _innerei = new ClaimExcInjection(cpObjCfg.ToArray());

            // manual copy list
            if (!objectConfig.Contains("checkpoints"))
            {
                target.CheckPoints.InjectFrom(_innerei, _claim.CheckPoints, "CheckPointID", true);
                var ids = _claim.CheckPoints.Select(x => x.CheckPointID);
                foreach (CheckPoint cp in target.CheckPoints)
                {
                    // Check for remove
                    if (!ids.Contains(cp.CheckPointID)) { cp.ObjectState = ObjectState.Deleted; continue; }

                    // Add new of Update
                    cp.ObjectState = cp.CheckPointID == 0 ? ObjectState.Added : ObjectState.Modified;
                }
            }
            if (!objectConfig.Contains("requirements"))
            {
                target.Requirements.InjectFrom(_innerei, _claim.Requirements, "RequirementID", true);
                var ids = _claim.Requirements.Select(x => x.RequirementID);
                foreach (Requirement rq in target.Requirements)
                {
                    // Check for remove
                    if (!ids.Contains(rq.RequirementID)) { rq.ObjectState = ObjectState.Deleted; continue; }

                    // Add new of Update
                    rq.ObjectState = rq.RequirementID == 0 ? ObjectState.Added : ObjectState.Modified;
                }
            }
            if (!objectConfig.Contains("payments"))
            {
                target.Payments.InjectFrom(_innerei, _claim.Payments, "PaymentID", true);
                var ids = _claim.Payments.Select(x => x.PaymentID);
                foreach (Payment pm in target.Payments)
                {
                    // Check for remove
                    if (!ids.Contains(pm.PaymentID)) { pm.ObjectState = ObjectState.Deleted; continue; }

                    // Add new of Update
                    pm.ObjectState = pm.PaymentID == 0 ? ObjectState.Added : ObjectState.Modified;
                }
            }
            if (!objectConfig.Contains("allocations"))
            {
                // Remove paymentID due not inject
                _innerei.ExcludeProps = _innerei.ExcludeProps.Where(x => !x.Equals("paymentID")).ToArray();
                target.Allocations.InjectFrom(_innerei, _claim.Allocations, "AllocationID", true);
                var ids = _claim.Allocations.Select(x => x.AllocationID);
                foreach (Allocation aloc in target.Allocations)
                {
                    // Check for remove
                    if (!ids.Contains(aloc.AllocationID)) { aloc.ObjectState = ObjectState.Deleted; continue; }

                    // Add new of Update
                    aloc.ObjectState = aloc.AllocationID == 0 ? ObjectState.Added : ObjectState.Modified;
                }
            }

            // Copy new upload file to Claim's folder
            // Manual copy Document because Document entity don't use any association with particular entity
            if (!objectConfig.Contains("documents"))
            {
                String dir = uploadPath + "/Claims/" + claim.ClaimID + "/";
                Directory.CreateDirectory(dir);
                foreach (DocumentDTO doc in claim.Documents)
                {
                    if ((doc.TempName ?? "") != "")
                    {
                        File.Move(uploadPath + "/Temps/" + doc.TempName, dir + doc.FileName);
                    }
                    doc.ReferenceID = claim.ClaimID;
                    doc.ReferenceName = "Claim";
                    doc.UploadTime = DateTime.Now;
                    doc.UploadBy = userID;
                }

                target.ClaimDocuments.InjectFrom(_innerei, claim.Documents, "DocumentID", true);
                var ids = claim.Documents.Select(x => x.DocumentID);

                foreach (Document doc in target.ClaimDocuments)
                {
                    // Check for remove
                    if (!ids.Contains(doc.DocumentID)) { doc.ObjectState = ObjectState.Deleted; continue; }

                    // Add new
                    doc.ObjectState = doc.DocumentID == 0 ? ObjectState.Added : ObjectState.Modified;
                }
            }


            // Check for other value to change status
            switch (action)
            {
                case "Save":
                    // Dont change anything, just SAVE depend on status, keep tracking in UPDATE Phase
                    if (target.StatusID >= 10 && target.StatusID <= 12)
                    {
                        DateTime now = DateTime.Now;
                        if (target.StartDate > now) target.StatusID = 10;       // PREPARE RUNNING
                        else if (target.EndDate > now) target.StatusID = 11;    // RUNNING
                        else target.StatusID = 12;                              // ENDING
                    }
                    break;
                case "Submit":
                    {
                        // Case: DRAFT -> PREPARE, RUNNING or ENDING
                        if (target.StatusID == 1) 
                        {
                            DateTime now = DateTime.Now;
                            if (target.StartDate > now) target.StatusID = 10;       // PREPARE RUNNING
                            else if (target.EndDate > now) target.StatusID = 11;    // RUNNING
                            else target.StatusID = 12;                              // ENDING
                            break;
                        }

                        // Case: ENDING -> WAITING APPROVE or SUBMITTED CLAIM
                        if (target.StatusID == 12)
                        {
                            target.StatusID = ((int)(target.PrePaid??0) == 1) ? (short)16 : (short)13;
                            break;
                        }

                        // Case: PREPARE CLAIM -> SUBMITTED CLAIM, SUBMITTED CLAIM -> SUBMITED CLAIM or DONE!
                        if (target.StatusID == 14)
                        {
                            if (target.VendorConfirmDate != null && (target.VendorConfirmAmount != 0)
                                && target.SubmitClaimDate != null)
                                target.StatusID = 15;
                            else
                                target.StatusID = 14;
                            break;
                        }
                        if (target.StatusID >= 15 && target.StatusID < 100)
                        {
                            if (target.Payments.Count() != 0)
                            {
                                target.StatusID = 25; 

                                // Check remain payment
                                var remainPayment = target.VendorConfirmAmount -
                                    target.Payments.Sum(x => x.VendorPayment);

                                // Check for complete 
                                if (remainPayment == 0)
                                {
                                    target.StatusID = 30;

                                    // Check total first
                                    var inCompleteAloc = target.Allocations.Where(
                                        x => x.AllocateAmount - x.CalculateAmount - x.ActualAmount > 0).Select(x => x);

                                    // Check for each Payment relate with Allocations
                                    if (inCompleteAloc.Count() == 0)
                                    {
                                        bool isAllocated = true;
                                        foreach (Payment pm in target.Payments)
                                        {
                                            var remainPm = pm.VendorPayment * pm.ExchangeRate
                                                - target.Allocations.Where(x => x.PaymentID == pm.PaymentID)
                                                .Sum(x => x.AllocateAmount);

                                            isAllocated = isAllocated && (Math.Abs(remainPm??0) <= 1);
                                        }

                                        if (isAllocated) target.StatusID = 100;
                                    }
                                }
                            }
                            break;
                        }
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

                case "RequestFail":
                    if (target.StatusID != 100)
                    {
                        // Save the previous status for restore when deny
                        target.PreviousStatus = target.StatusID;
                        target.StatusID = 20;
                    }
                    break;
                case "ApproveFail":
                    if (target.StatusID == 20) target.StatusID = 99;
                    break;
                case "DenyFail":
                    if (target.StatusID == 20) target.StatusID = target.PreviousStatus;
                    break;
            }

            target.ObjectState = ObjectState.Modified;
            _unitOfWorkAsync.RepositoryAsync<Claim>().Update(target);
            var claimId = await _unitOfWorkAsync.SaveChangesAsync();

            return AutoMapper.Mapper.Map<Claim, ClaimDTO>(target);
        }
    }
}
