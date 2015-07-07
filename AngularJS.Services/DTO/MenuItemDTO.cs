using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.DTO
{
    public class MenuItemDTO
    {
        public int ID { get; set; }
		public int ParentID { get; set; }
        public int Href { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
    }
}
