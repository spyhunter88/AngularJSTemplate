using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AngularJS.Entities.Models;
using AngularJS.Service;
using AngularJS.Services.DTO;
using AngularJS.Web.Models;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Web.Api
{
    public class ClaimController : ApiController
    {
        // private AngularJSContext db = new AngularJSContext();
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IClaimService _claimSerivce;

        public ClaimController(IUnitOfWorkAsync unitOfWorkAsync, IClaimService claimSerivce)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _claimSerivce = claimSerivce;
        }
        //// GET api/Claim
        //[Queryable]
        //public IQueryable<Claim> GetClaims()
        //{
        //    var claims = _unitOfWorkAsync.RepositoryAsync<Claim>().Queryable();
        //    // List<Claim> _claims = claims;

        //    return claims;
        //}

        [HttpGet]
        [Authorize]
        [ResponseType(typeof(ClaimListViewModels))]
        public IHttpActionResult GetClaims([FromUri]ClaimFilter filterCriteria)
        {
            int totalCount = 0;
            if (filterCriteria.SortCol == null) filterCriteria.SortCol = "CreateTime";

            List<ClaimLiteDTO> claims = _claimSerivce.SearchClaim(filterCriteria.FullSearch,
                filterCriteria.SortCol, filterCriteria.SortDir,
                filterCriteria.PageSize, filterCriteria.Page, out totalCount);

            ClaimListViewModels _claimViewModel = new ClaimListViewModels();
            _claimViewModel.Total = totalCount;
            _claimViewModel.Claims = claims;

            return Ok(_claimViewModel);
        }

        //
        // GET api/Claim/5
        [Authorize]
        [ResponseType(typeof(Claim))]
        public async Task<IHttpActionResult> GetClaim(int id)
        {
            // Claim claim = await _unitOfWorkAsync.RepositoryAsync<Claim>().FindAsync(id);
            ClaimDTO claim = await _claimSerivce.GetClaimAsync(id);

            if (claim == null)
            {
                return NotFound();
            }

            return Ok(claim);
        }

        //
        // PUT api/Claim/5
        public async Task<IHttpActionResult> PutClaim(int id, Claim claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != claim.ClaimID)
            {
                return BadRequest();
            }

            Claim _claim = await _unitOfWorkAsync.RepositoryAsync<Claim>().FindAsync(id);

            _claim.ObjectState = ObjectState.Added;



            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //
        // POST api/Claim
        [ResponseType(typeof(Claim))]
        public async Task<IHttpActionResult> PostClaim(ClaimDTO claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int newId = await _claimSerivce.PostClaim(claim);

            return CreatedAtRoute("DefaultApi", new { id = claim.ClaimID }, claim);
        }

        //
        // DELETE api/Claim/5
        [ResponseType(typeof(Claim))]
        public async Task<IHttpActionResult> DeleteClaim(int id)
        {
            Claim claim = await _unitOfWorkAsync.RepositoryAsync<Claim>().FindAsync(id);
            if (claim == null)
            {
                return NotFound();
            }

            claim.ObjectState = ObjectState.Deleted;
            _unitOfWorkAsync.RepositoryAsync<Claim>().Delete(claim);
            await _unitOfWorkAsync.SaveChangesAsync();

            return Ok(claim);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWorkAsync.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClaimExists(int id)
        {
            return _unitOfWorkAsync.RepositoryAsync<Claim>().Query(e => e.ClaimID == id).Select().Count() > 0;
        }
    }
}