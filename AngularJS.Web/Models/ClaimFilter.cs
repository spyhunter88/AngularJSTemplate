using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJS.Web.Models
{
    public class SortDirection
    {
        public SortDirection()
        {
            Col = String.Empty;
            Reverse = false;
        }

        public string Col { get; set; }
        public bool Reverse { get; set; }
    }

    public class ClaimFilter
    {
        public ClaimFilter()
        {
            FullSearch = String.Empty;
            Vendor = String.Empty;
            Bu = String.Empty;
            ProgramType = String.Empty;
            Participant = String.Empty;
        }

        public string FullSearch { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[] Status { get; set; }
        public string Vendor { get; set; }
        public string Bu { get; set; }
        public string ProgramType { get; set; }
        public string Participant { get; set; }
        // public SortDirection[] SortDir { get; set; }
        public string SortCol { get; set; }
        public bool SortDir { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }
}