using System.Collections.Generic;
using AngularJS.Entities.Models;

namespace AngularJS.Service
{
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly IAngularJSStoredProcedures _storedProcedures;

        public StoredProcedureService(IAngularJSStoredProcedures storedProcedures)
        {
            _storedProcedures = storedProcedures;
        }

        public IEnumerable<CustomerOrderHistory> CustomerOrderHistory(string customerID)
        {
            return _storedProcedures.CustomerOrderHistory(customerID);
        }

        public int CustOrdersDetail(int? orderID)
        {
            return _storedProcedures.CustOrdersDetail(orderID);
        }

        public IEnumerable<CustomerOrderDetail> CustomerOrderDetail(string customerID)
        {
            return _storedProcedures.CustomerOrderDetail(customerID);
        }
    }
}