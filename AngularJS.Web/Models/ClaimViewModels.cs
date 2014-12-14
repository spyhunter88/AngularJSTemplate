using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AngularJS.Entities.Models;
using AngularJS.Services.DTO;

namespace AngularJS.Web.Models
{
    public class ClaimListViewModels
    {
        public List<ClaimLiteDTO> Claims { get; set; }

        public int Total { get; set; }
    }
}