using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.DTO
{
    public class RequirementDTO
    {

        public int RequirementID { get; set; }
        public int ClaimID { get; set; }
        public String Name { get; set; }
        public String Operation { get; set; }
        public Int16? Target { get; set; }
        public String Unit { get; set; }
        public Int32? ActualAmount { get; set; }
        public Int32? ClaimAmount { get; set; }
        public DateTime? ReportDate { get; set; }
        public String Note { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Int16? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        // Extra
        public String CreateUser { get; set; }
        public String LastEditUser { get; set; }
    }
}
