using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using AngularJS.Entities.Models;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Web.Api
{
    public class ProductLineController : BaseController
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public ProductLineController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        //
        // GET: api/ProductLine
        [ResponseType(typeof(List<ProductLine>))]
        public IHttpActionResult Get([FromUri]string type)
        {
            List<ProductLine> _result = new List<ProductLine>();

            _result = _unitOfWorkAsync.Repository<ProductLine>().Query().Select().ToList();

            if (_result != null)
            {
                return Ok(_result);
            }
            return NotFound();
        }
    }
}
