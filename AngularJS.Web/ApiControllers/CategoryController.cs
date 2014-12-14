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
using System.Collections.Generic;


namespace AngularJS.Web.Api
{
    public class CategoryController : ApiController
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public CategoryController(IUnitOfWorkAsync unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        //
        // GET: api/Category
        [ResponseType(typeof(List<Category>))]
        public IHttpActionResult Get([FromUri]string type)
        {
            List<Category> _result = new List<Category>();

            if (type == null || type == "")
            {
                _result = _unitOfWorkAsync.Repository<Category>().Query().Select().ToList();
            }
            else
            {
                _result = _unitOfWorkAsync.Repository<Category>()
                    .Query(x => x.Type == type)
                    .Select().ToList();
            }

            if (_result != null)
            {
                return Ok(_result);
            }
            return NotFound();
        }

        //
        // GET: api/Category/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult Get(int id)
        {
            var _result = _unitOfWorkAsync.Repository<Category>().Find(id);
            return Ok(_result);
        }

        // POST: api/Category
        public void Post([FromBody]Category value)
        {
            value.CategoryID = 0;

            _unitOfWorkAsync.Repository<Category>().Insert(value);
        }

        // PUT: api/Category/5
        public void Put(int id, [FromBody]Category value)
        {
        }

        // DELETE: api/Category/5
        public void Delete(int id)
        {
            _unitOfWorkAsync.Repository<Category>().Delete(id);
        }
    }
}
