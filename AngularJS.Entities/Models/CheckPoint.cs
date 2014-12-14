using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class CheckPoint : Entity
    {
        public CheckPoint() 
        {
            this.RequirementHistories = new List<RequirementHistory>();
        }


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


        // Mapping
        public virtual Claim Claim { internal get; set; }
        public virtual List<RequirementHistory> RequirementHistories { get; set; }
    }
}
