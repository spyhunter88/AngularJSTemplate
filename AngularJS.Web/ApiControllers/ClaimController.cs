using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AngularJS.Entities.Models;
using AngularJS.Service;
using AngularJS.Services.DTO;
using AngularJS.Web.Models;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using AngularJS.Web.Security.Models;
using AngularJS.Services;

namespace AngularJS.Web.Api
{
    public class ClaimController : BaseController
    {
        // private AngularJSContext db = new AngularJSContext();
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IClaimService _claimSerivce;
        private readonly IObjectConfigService _objectConfigService;
        private readonly string objectName = "Claim";

        public ClaimController(IUnitOfWorkAsync unitOfWorkAsync, IClaimService claimSerivce, IObjectConfigService objectConfigService)
            : base()
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _claimSerivce = claimSerivce;
            _objectConfigService = objectConfigService;
        }

        // 
        // GET api/Claim
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
        [ResponseType(typeof(ClaimDTO))]
        public async Task<IHttpActionResult> GetClaim(int id)
        {
            ClaimDTO claim = null;
            try
            {
                // Get ClaimLiteDTO with policy
                var _claimLite = _claimSerivce.GetClaimLite(id);

                // 2. Get policy
                int _userID = GetCurrentUserId();
                var _objectAction = _objectConfigService.GetObjectAction(_userID, objectName, _claimLite.Phase, _claimLite.CreateBy == _userID);
                var _objectConfig = _objectConfigService.GetObjectConfig(_userID, objectName, _claimLite.Phase, _claimLite.CreateBy == _userID);

                // 3. Get claim with policy inside
                claim = await _claimSerivce.GetClaimAsync(id, _objectConfig);
                

                claim.SetObjectConfig(_objectAction, _objectConfig);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            if (claim == null)
            {
                return NotFound();
            }

            return Ok(claim);
        }

        //
        // PUT api/Claim/5
        [HttpPut]
        public async Task<IHttpActionResult> PutClaim([FromUri]int id, [FromBody]ClaimDTO claim)
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

        // Use for both Create New and Update
        // POST api/Claim
        [Authorize]
        [ResponseType(typeof(ClaimDTO))]
        public async Task<IHttpActionResult> PostClaim([FromBody]ClaimDTO claim, [FromUri]String action)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationUser _user = GetCurrentUser();

                string uploadPath = GetUploadPath();
                claim.CreateBy = GetCurrentUserId();
                claim.CreateTime = DateTime.Now;

                if (claim.Documents != null && claim.Documents.Count > 0)
                {
                    foreach (DocumentDTO _doc in claim.Documents)
                    {
                        _doc.UploadBy = (int)claim.CreateBy;
                        _doc.UploadTime = claim.CreateTime;
                    }
                } 
                else
                {
                    claim.Documents = new List<DocumentDTO>();
                }

                if (claim.ClaimID == 0)
                {
                    var newID = await _claimSerivce.PostClaim(claim, uploadPath);
                    claim.ClaimID = newID;
                }
                else
                    claim = await _claimSerivce.PutClaim(claim, uploadPath, _user.Id, action);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return Ok(claim);
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