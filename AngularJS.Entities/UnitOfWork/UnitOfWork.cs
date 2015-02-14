using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6.Factories;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;

namespace AngularJS.Entities.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Base
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
        #endregion

        #region Async
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState;
        #endregion
    }

    public class UnitOfWork : Repository.Pattern.Ef6.UnitOfWork
    {
        public UnitOfWork(IDataContextAsync dataContext, IRepositoryProvider repositoryProvider)
            : base(dataContext, repositoryProvider)
        { }

        #region Override Base Methods
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}
