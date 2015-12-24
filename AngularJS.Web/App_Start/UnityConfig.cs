using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Repository.Pattern.DataContext;
using AngularJS.Entities.Models;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using AngularJS.Service;
using AngularJS.Services;

namespace AngularJS.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container
                .RegisterType<IDataContextAsync, AngularJSContext>(new PerRequestLifetimeManager())
                .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestLifetimeManager())
                .RegisterType<IRepositoryAsync<Customer>, Repository<Customer>>()
                .RegisterType<IRepositoryAsync<Product>, Repository<Product>>()
                .RegisterType<IRepositoryAsync<Claim>, Repository<Claim>>()
                .RegisterType<IRepositoryAsync<Request>, Repository<Request>>()
                .RegisterType<IRepositoryAsync<ProductLine>, Repository<ProductLine>>()
                .RegisterType<IRepositoryAsync<Vendor>, Repository<Vendor>>()
                .RegisterType<IRepositoryAsync<ObjectAction>, Repository<ObjectAction>>()
                .RegisterType<IRepositoryAsync<ObjectConfig>, Repository<ObjectConfig>>()
                .RegisterType<IRepositoryAsync<User>, Repository<User>>()
                .RegisterType<IRepositoryAsync<ClaimStatus>, Repository<ClaimStatus>>()
                .RegisterType<IRepositoryAsync<Category>, Repository<Category>>()

                .RegisterType<IMenuService, MenuService>()
                .RegisterType<IProductService, ProductService>()
                .RegisterType<ICustomerService, CustomerService>()
                .RegisterType<IClaimService, ClaimService>()
                .RegisterType<IObjectConfigService, ObjectConfigService>()
                .RegisterType<ICategoryService, CategoryService>()

                .RegisterType<IAngularJSStoredProcedures, AngularJSContext>(new PerRequestLifetimeManager())
                .RegisterType<IStoredProcedureService, StoredProcedureService>();
        }
    }
}
