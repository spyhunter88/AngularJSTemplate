using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class Requirement : Entity
    {
        public Requirement() 
        {
            this.RequirementHistories = new List<RequirementHistory>();
        }

        public int RequirementID { get; set; }
        public int ClaimID { get; set; }
        public String Name { get; set; }
        public String Operation { get; set; }
        public Int32? Target { get; set; }
        public String Unit { get; set; }
        public Int32? ActualAmount { get; set; }
        public Int32? ClaimAmount { get; set; }
        public DateTime? ReportDate { get; set; }
        public String Note { get; set; }
        public Int16? CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Int16? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        // Mapping
        public virtual Claim Claim { internal get; set; }
        public virtual List<RequirementHistory> RequirementHistories { get; set; }
    }
}
