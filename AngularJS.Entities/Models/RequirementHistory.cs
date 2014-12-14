using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class RequirementHistory : Entity
    {
        public RequirementHistory() { }

        public int ID { get; set; }
        public int RequirementID { get; set; }
        public int CheckPointID { get; set; }
        public DateTime? UpdateTime { get; set; }
        public Int32? UpdateAmount { get; set; }
        public String UpdateNote { get; set; }
        public int CreateBy { get; set; }
        public DateTime? CreateTime { get; set; }
        public Int16? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        // Mapping
        public virtual Requirement Requirement { internal get; set; }
        public virtual CheckPoint CheckPoint { internal get; set; }
    }
}
