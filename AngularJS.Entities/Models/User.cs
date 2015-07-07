using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public class User : Entity
    {
        public User()
        {
            this.Roles = new HashSet<Role>();
			this.MenuItems = new List<MenuItem>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }

        public ICollection<Role> Roles { get; set; }
		public virtual ICollection<MenuItem> MenuItems { get; set; }
    }

    public class Role : Entity
    {
        public Role()
        {
            this.Users = new HashSet<User>();
            // this.MenuItems = new List<MenuItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
