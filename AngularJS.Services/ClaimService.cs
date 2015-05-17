using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AngularJS.Entities.Models;
using AngularJS.Entities;
using AngularJS.Services.DTO;
using AutoMapper;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System.IO;
using Omu.ValueInjecter;
using AngularJS.Services.InjectConfig;

namespace AngularJS.Service
{
    public interface IClaimService
    {
        List<ClaimLiteDTO> SearchClaim(String searchText, string orderBy, bool descending, int pageSize, int page, out int totalCount);

        Task<ClaimDTO> GetClaimAsync(int id);

        Task<int> PostClaim(ClaimDTO claim, string uploadPath);

        Task<ClaimDTO> PutClaim(ClaimDTO claim, string uploadPath);
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
            List<Claim> _claims = new List<Claim>(_unitOfWorkAsync.RepositoryAsync<Claim>().Query(x => x.FtgProgramCode.Contains(searchText) || x.ProgramName.Contains(searchText))
                                    .OrderBy(q => q.OrderBy(orderBy, descending))
                                    .SelectPage(page, pageSize, out totalCount)
            );

            Random rand = new Random(100);

            List<ClaimLiteDTO> result = AutoMapper.Mapper.Map<List<Claim>, List<ClaimLiteDTO>>(_claims);

            // Add Extra information
            foreach (ClaimLiteDTO claim in result)
            {
                claim.CreateUser = "LinhNH";
                claim.LastEditUser = "LinhNH13";
                claim.RemainPayment = rand.Next(0, 1000000);
                claim.RemainAllocation = rand.Next(0, 10000000);
            }

            return result;
        }

        public async Task<ClaimDTO> GetClaimAsync(int id)
        {
            var claims = await _unitOfWorkAsync.RepositoryAsync<Claim>().Query(x => x.ClaimID == id)
                .Include(x => x.CheckPoints)
                .Include(x => x.Requirements)
                .SelectAsync();
            var _claim = claims.FirstOrDefault();

            // Transform Claim to ClaimDTO and copy other list
            ClaimDTO claim = AutoMapper.Mapper.Map<Claim, ClaimDTO>(_claim);
            claim.CheckPoints = AutoMapper.Mapper.Map<ICollection<CheckPoint>, List<CheckPointDTO>>(_claim.CheckPoints);
            claim.Requirements = AutoMapper.Mapper.Map<ICollection<Requirement>, List<RequirementDTO>>(_claim.Requirements);
            


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
        /// Use to update claim from workflow
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="uploadPath"></param>
        /// <returns></returns>
        public async Task<ClaimDTO> PutClaim(ClaimDTO claim, string uploadPath)
        {
            Claim _claim = AutoMapper.Mapper.Map<ClaimDTO, Claim>(claim);
            _claim.CheckPoints = AutoMapper.Mapper.Map<List<CheckPointDTO>, ICollection<CheckPoint>>(claim.CheckPoints);
            _claim.Requirements = AutoMapper.Mapper.Map<List<RequirementDTO>, ICollection<Requirement>>(claim.Requirements);

            ClaimInjection cu = new ClaimInjection(new string[] { "ProgramContent", "EndDate" });

            var targets = await _unitOfWorkAsync.RepositoryAsync<Claim>().Query(x => x.ClaimID == _claim.ClaimID).SelectAsync();
            var target = targets.FirstOrDefault();

            target.InjectFrom(cu, _claim);

            _unitOfWorkAsync.RepositoryAsync<Claim>().Update(target);
            var claimId = _unitOfWorkAsync.SaveChangesAsync();

            return AutoMapper.Mapper.Map<Claim, ClaimDTO>(target);
        }
    }
}
