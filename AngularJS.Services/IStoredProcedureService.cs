#region

using System.Collections.Generic;
using AngularJS.Entities.Models;

#endregion

namespace AngularJS.Service
{
    public interface IStoredProcedureService
    {
        IEnumerable<CustomerOrderHistory> CustomerOrderHistory(string customerID);
        int CustOrdersDetail(int? orderID);
        IEnumerable<CustomerOrderDetail> CustomerOrderDetail(string customerID);
    }
}