using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AngularJS.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using AngularJS.Entities;
using AngularJS.Services.DTO;
using AutoMapper;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Service
{
    public interface IClaimService
    {
        List<ClaimLiteDTO> SearchClaim(String searchText, string orderBy, bool descending, int pageSize, int page, out int totalCount);

        Task<ClaimDTO> GetClaimAsync(int id);

        Task<int> PostClaim(ClaimDTO claim);
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

            List<ClaimLiteDTO> result = Mapper.Map<List<Claim>, List<ClaimLiteDTO>>(_claims);

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
                // .Include(x => x.CheckPoints)
                // .Include(x => x.Requirements)
                .SelectAsync();
            var _claim = claims.FirstOrDefault();

            // Transform Claim to ClaimDTO and copy other list
            ClaimDTO claim = Mapper.Map<Claim, ClaimDTO>(_claim);
            claim.CheckPoints = Mapper.Map<ICollection<CheckPoint>, List<CheckPointDTO>>(_claim.CheckPoints);
            claim.Requirements = Mapper.Map<ICollection<Requirement>, List<RequirementDTO>>(_claim.Requirements);
            // claim.CheckPoints = _claim.CheckPoints.OrderBy(x => x.CheckDate).ToList();
            // claim.Requirements = _claim.Requirements.ToList();

            return claim;
        }

        /// <summary>
        /// Create new Claim, include CheckPoints, Requirements and Documents' path
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        public async Task<int> PostClaim(ClaimDTO claim)
        {
            Claim _claim = Mapper.Map<ClaimDTO, Claim>(claim);
            _claim.CheckPoints = Mapper.Map<List<CheckPointDTO>, ICollection<CheckPoint>>(claim.CheckPoints);
            _claim.Requirements = Mapper.Map<List<RequirementDTO>, ICollection<Requirement>>(claim.Requirements);

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
            int newId = await _unitOfWorkAsync.SaveChangesAsync();

            // TODO: Rename and move documents to Claim's folder


            return newId;
        }
    }
}
