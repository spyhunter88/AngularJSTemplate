using System.Collections.Generic;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class MenuItem : Entity
    {
		public MenuItem() 
		{
			this.Users = new List<User>();
            // this.Roles = new List<Role>();
		}
		
        public int ID { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
		public string Route { get; set; }
        public int? ParentID { get; set; }
        public string Module { get; set; }
		
		public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
