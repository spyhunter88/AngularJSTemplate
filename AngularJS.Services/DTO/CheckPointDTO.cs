using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.DTO
{
    public class CheckPointDTO
    {

        public int CheckPointID { get; set; }
        public int ClaimID { get; set; }
        public DateTime? CheckDate { get; set; }
        public DateTime? WarningDate { get; set; }
        public DateTime? ReportDate { get; set; }
        public String Action { get; set; }
        public String Note { get; set; }
        public int SendMailCount { get; set; }
        public int SendMailMax { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Int16? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        // Extra
        public String CreateUser { get; set; }
        public String LastEditUser { get; set; }
    }
}
