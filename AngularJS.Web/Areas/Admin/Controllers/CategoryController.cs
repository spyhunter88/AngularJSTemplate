using AngularJS.Entities.Models;
using AngularJS.Services;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;

namespace AngularJS.Web.Areas.Admin.Controllers
{
    public class CategoryController : ODataController
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly ICategoryService _categoryService;

        public CategoryController(IUnitOfWorkAsync unitOfWorkAsync, ICategoryService categoryService)
        {
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._categoryService = categoryService;
        }

		// GET: odata/Categories
		[HttpGet]
		[Queryable]
		public IQueryable<Category> GetCategory()
		{
			return _categoryService.Queryable();
		}
		
		// GET: odata/Categories(5)
		[Queryable]
		public SingleResult<Category> GetCategory([FromODataUri] int key)
		{
			return SingleResult.Create(_categoryService.Queryable().Where(t => t.CategoryID == key));
		}
		
		// PUT: odata/Categories(5)
		public async Task<IHttpActionResult> Put(int key, Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			
			if (key != category.CategoryID)
			{
				return BadRequest();
			}
			
			category.ObjectState = ObjectState.Modified;
			_categoryService.Update(category);
			
			try
			{
				await _unitOfWorkAsync.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(key))
				{
					return NotFound();
				}
				throw;
			}
			
			return Updated(category);
		}
		
		// POST: odata/Categories
		public async Task<IHttpActionResult> Post(Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			
			category.ObjectState = ObjectState.Added;
			_categoryService.Insert(category);
			
			try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.CategoryID))
                {
                    return Conflict();
                }
                throw;
            }

            return Created(category);
		}
		
		// PATCH: odata/Categories(5)
		[AcceptVerbs("PATCH", "MERGE")]
		public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Category> patch)
		{
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = await _categoryService.FindAsync(key);

            if (category == null)
            {
                return NotFound();
            }

            patch.Patch(category);
            category.ObjectState = ObjectState.Modified;

            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(category);
		}
		
		// DELETE: odata/Categories(5)
		public async Task<IHttpActionResult> Delete(int key)
		{
			Category category = await _categoryService.FindAsync(key);
			
			if (category == null)
			{
				return NotFound();
			}
			
			category.ObjectState = ObjectState.Deleted;
			_categoryService.Delete(category);
			await _unitOfWorkAsync.SaveChangesAsync();
			
			return StatusCode(HttpStatusCode.NoContent);
		}


        private bool CategoryExists(int key)
        {
            return _categoryService.Query(e => e.CategoryID == key).Select().Any();
        }
    }
}