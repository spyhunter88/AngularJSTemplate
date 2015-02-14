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
        public User() { }

        public int Id { get; set; }
        public string UserName { get; set; }

        public string ToString()
        {
            return UserName ?? Id.ToString();
        }
    }
}
