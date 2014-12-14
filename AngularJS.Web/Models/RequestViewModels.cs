using AngularJS.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJS.Web.Models
{
    public class RequestViewModels
    {
        public List<Request> Requests { get; set; }

        public int Total { get; set; }
    }
}