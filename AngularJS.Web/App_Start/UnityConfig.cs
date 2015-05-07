using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularJS.Entities.Models;
using AngularJS.Service;
using AngularJS.Services;
using Microsoft.Practices.Unity;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Ef6.Factories;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;

namespace AngularJS.Web
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
                .RegisterType<IRepositoryProvider, RepositoryProvider>(
                    new PerRequestLifetimeManager(),
                    new InjectionConstructor(new object[] { new RepositoryFactories() })
                    )
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

                .RegisterType<IProductService, ProductService>()
                .RegisterType<ICustomerService, CustomerService>()
                .RegisterType<IClaimService, ClaimService>()
                .RegisterType<IObjectConfigService, ObjectConfigService>()

                .RegisterType<IAngularJSStoredProcedures, AngularJSContext>(new PerRequestLifetimeManager())
                .RegisterType<IStoredProcedureService, StoredProcedureService>();
        }
    }
}