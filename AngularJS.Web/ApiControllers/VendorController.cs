using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using AngularJS.Entities.Models;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Web.Api
{
    public class VendorController : BaseController
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public VendorController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        //
        // GET: api/Vendor
        [ResponseType(typeof(List<Vendor>))]
        public IHttpActionResult Get([FromUri]string type)
        {
            List<Vendor> _result = new List<Vendor>();

            _result = _unitOfWorkAsync.Repository<Vendor>().Query().Select().ToList();

            if (_result != null)
            {
                return Ok(_result);
            }
            return NotFound();
        }
    }
}
