﻿#region

using System.Collections;
using System.Collections.Generic;
using AngularJS.Entities.Models;
using AngularJS.Repository.Models;
using AngularJS.Repository.Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;

#endregion

namespace AngularJS.Service
{
    public interface ICustomerService : IService<Customer>
    {
        // Add any custom business logic (methods) here
        // All methods in Service<TEntity> are ovverridable for any custom implementations
        decimal CustomerOrderTotalByYear(string customerId, int year);
        IEnumerable<Customer> CustomersByCompany(string companyName);
        IEnumerable<CustomerOrder> GetCustomerOrder(string country);
    }

    // Add any custom business logic (methods) here
    // All methods in Service<TEntity> are ovverridable for any custom implementations
    // Can ovveride any of the Repository methods to add business logic in them
    // e.g.
    //public override void Delete(Customer entity)
    //{
    //    // Add business logic before or after deleting entity.
    //    base.Delete(entity);
    //}
    public class CustomerService : Service<Customer>, ICustomerService
    {
        private readonly IRepositoryAsync<Customer> _repository;

        public CustomerService(IRepositoryAsync<Customer> repository) : base(repository)
        {
            _repository = repository;
        }

        public decimal CustomerOrderTotalByYear(string customerId, int year)
        {
            return _repository.GetCustomerOrderTotalByYear(customerId, year);
        }

        public IEnumerable<Customer> CustomersByCompany(string companyName)
        {
            return _repository.CustomersByCompany(companyName);
        }

        public IEnumerable<CustomerOrder> GetCustomerOrder(string country)
        {
            return _repository.GetCustomerOrder(country);
        }
    }
}