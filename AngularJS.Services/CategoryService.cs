#region
using AngularJS.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;

#endregion

namespace AngularJS.Services
{
    public interface ICategoryService : IService<Category>
    {

    }

    public class CategoryService : Service<Category>, ICategoryService
    {
        private readonly IRepositoryAsync<Category> _repository;

        public CategoryService(IRepositoryAsync<Category> repository) : base(repository)
        {
            _repository = repository;
        }


    }
}
