using AngularJS.Entities.Models;
using AngularJS.Web.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace AngularJS.Web.Api
{
    public class RequestController : ApiController
    {

        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public RequestController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            this._unitOfWorkAsync = unitOfWorkAsync;
        }

        [Authorize]
        [HttpGet]
        [ResponseType(typeof(RequestViewModels))]
        public IHttpActionResult GetRequests()
        {
            RequestViewModels _requestViewModel = new RequestViewModels();

            _requestViewModel.Requests = _unitOfWorkAsync.RepositoryAsync<Request>().Query().Select().ToList();

            _requestViewModel.Total = 20;

            return Ok(_requestViewModel);
        }
    }
}
