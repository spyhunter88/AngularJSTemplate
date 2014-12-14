using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Entities.Models
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public int? ParentID { get; set; }
    }
}
