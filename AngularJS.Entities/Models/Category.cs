using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public partial class Category : Entity
    {
        public Category() {}

        public int CategoryID { get; set; }
        public String Type { get; set; }
        public String Code { get; set; }
        public String Value { get; set; }
        public String Description { get; set; }
    }
}
