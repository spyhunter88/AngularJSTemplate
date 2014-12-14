using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.DTO
{
    public class FeedDTO
    {
        public FeedDTO() { }
 
        public String Title { get; set; }
        public String SubTitle { get; set; }
        public String Content { get; set; }
        public String UserName { get; set; }
        public String UpdateTime { get; set; }
        public String UpdateType { get; set; }
        public String Url { get; set; }
    }
}
