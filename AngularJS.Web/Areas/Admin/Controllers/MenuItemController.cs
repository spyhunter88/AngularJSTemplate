using AngularJS.Entities.Models;
using AngularJS.Service;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData;

namespace AngularJS.Web.Areas.Admin.Controllers
{
    public class MenuItemController : ODataController
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IMenuService _menuService;

        public MenuItemController(IUnitOfWorkAsync unitOfWorkAsync, IMenuService menuService)
        {
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._menuService = menuService;
        }

        // GET: odata/MenuItem
        [HttpGet]
        [Queryable]
        public IQueryable<MenuItem> GetMenuItem()
        {
            return _menuService.Queryable();
        }

        // GET: odata/MenuItem(5)
        [Queryable]
        public SingleResult<MenuItem> GetMenuItem([FromODataUri] int key)
        {
            return SingleResult.Create(_menuService.Queryable().Where(t => t.ID == key));
        }

        // PUT: odata/MenuItem(5)
        public async Task<IHttpActionResult> Put(int key, MenuItem menuItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != menuItem.ID)
            {
                return BadRequest();
            }

            menuItem.ObjectState = ObjectState.Modified;
            _menuService.Update(menuItem);

            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuItemExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(menuItem);
        }

        // POST: odata/MenuItem
        public async Task<IHttpActionResult> Post(MenuItem menuItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // kendo can not send empty when use inline-edit
            if (menuItem.ParentID == 0) menuItem.ParentID = null;

            menuItem.ObjectState = ObjectState.Added;
            _menuService.Insert(menuItem);

            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MenuItemExists(menuItem.ID))
                {
                    return Conflict();
                }
                throw;
            }

            return Created(menuItem);
        }

        // PATCH: odata/MenuItem(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<MenuItem> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MenuItem menuItem = await _menuService.FindAsync(key);

            if (menuItem == null)
            {
                return NotFound();
            }

            patch.Patch(menuItem);
            menuItem.ObjectState = ObjectState.Modified;

            try
            {
                await _unitOfWorkAsync.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuItemExists(key))
                {
                    return NotFound();
                }
                throw;
            }

            return Updated(menuItem);
        }

        // DELETE: odata/MenuItem(5)
        public async Task<IHttpActionResult> Delete(int key)
        {
            MenuItem menuItem = await _menuService.FindAsync(key);

            if (menuItem == null)
            {
                return NotFound();
            }

            menuItem.ObjectState = ObjectState.Deleted;
            _menuService.Delete(menuItem);
            await _unitOfWorkAsync.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }


        private bool MenuItemExists(int key)
        {
            return _menuService.Query(e => e.ID == key).Select().Any();
        }
    }
}