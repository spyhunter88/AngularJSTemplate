﻿using System.Collections.Generic;
using System.Linq;
using AngularJS.Entities.Models;
using AngularJS.Repository.Models;
using Repository.Pattern.Repositories;

namespace AngularJS.Repository.Repositories
{
    // Exmaple: How to add custom methods to a repository.
    public static class CustomerRepository
    {
        public static decimal GetCustomerOrderTotalByYear(this IRepository<Customer> repository, string customerId,
            int year)
        {
            return repository
                .Find(customerId)
                .Orders.SelectMany(o => o.OrderDetails)
                .Select(o => o.Quantity * o.UnitPrice)
                .Sum();
        }

        public static IEnumerable<Customer> CustomersByCompany(this IRepositoryAsync<Customer> repository,
            string companyName)
        {
            return repository
                .Queryable()
                .Where(x => x.CompanyName.Contains(companyName))
                .AsEnumerable();
        }

        public static IEnumerable<CustomerOrder> GetCustomerOrder(this IRepository<Customer> repository, string country)
        {
            var customers = repository.GetRepository<Customer>().Queryable();
            var orders = repository.GetRepository<Order>().Queryable();

            var query = from c in customers
                        join o in orders on new { a = c.CustomerID, b = c.Country }
                            equals new { a = o.CustomerID, b = country }
                        select new CustomerOrder
                        {
                            CustomerId = c.CustomerID,
                            ContactName = c.ContactName,
                            OrderId = o.OrderID,
                            OrderDate = o.OrderDate
                        };

            return query.AsEnumerable();
        }
    }
}