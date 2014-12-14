using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngularJS.Entities.Models;

namespace AngularJS.Services.DTO
{
    public class ClaimLiteDTO
    {
        public int ClaimID { get; set; }
        public Int16? StatusID { get; set; }
        public String ClaimType { get; set; }
        public String BuName { get; set; }
        public String UnitClaim { get; set; }
        public String FtgProgramCode { get; set; }
        public String ProgramName { get; set; }
        public String ProgramType { get; set; }
        public String ProgramContent { get; set; }
        public String VendorName { get; set; }
        public String ProductLine { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClaimDeadlineDate { get; set; }

        public Int16? CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public Int16? LastEditBy { get; set; }
        public DateTime? LastEditTime { get; set; }

        // Extra
        public String Status { get; set; }
        public String CreateUser { get; set; }
        public String LastEditUser { get; set; }
        public Int32 RemainPayment { get; set; }
        public Int32 RemainAllocation { get; set; }
    }

    public class ClaimDTO
    {
        public int ClaimID { get; set; }
        public Int32? RequestID { get; set; }
        public Int16? StatusID { get; set; }
        public string EditStatus { get; set; }
        public string ClaimType { get; set; }
        public string BuName { get; set; }
        public string ParticipantClaim { get; set; }
        public string UnitClaim { get; set; }
        public string ReceiptAccount { get; set; }
        public string FtgProgramCode { get; set; }
        public string VendorProgramCode { get; set; }
		public string ProgramName { get; set; }
        public string ProgramType { get; set; }
        public string PaymentMethod { get; set; }
        public Int16? RequireClaimDoc { get; set; }
        public string VendorName { get; set; }
		public string ProductLine { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ClaimDeadlineDate { get; set; }
        public Int16? PrePaid { get; set; }
        public Int16? PreviousStatus { get; set; }
        public string Note { get; set; }
        public string ProgramContent { get; set; }
		public string CurrencyUnit { get; set; }
        public Decimal? ExchangeRate { get; set; }
        public string ClaimItem { get; set; }
        public Decimal? PromiseAmount { get; set; }
		public DateTime? PromiseDate { get; set; }
        public Decimal? SubmitClaimAmount { get; set; }
		public DateTime? SubmitClaimDate { get; set; }
        public Decimal? VendorConfirmAmount { get; set; }
		public DateTime? VendorConfirmDate { get; set; }
        public Decimal? CurrentAvailableAmount { get; set; }
		public DateTime? CurrentAvailableDate { get; set; }
        public string EmailsReceiver { get; set; }
		
        public Int16? CreateBy { get; set; }
		public DateTime CreateTime { get; set; }
		public Int16? LastEditBy { get; set; }
		public DateTime? LastEditTime { get; set; }
        public Int16? LastApproveBy { get; set; }
		public DateTime? LastApproveTime { get; set; }

        // Extra
        public String Status { get; set; }
        public String CreateUser { get; set; }
        public String LastEditUser { get; set; }
		public String LastApproveUser { get; set; }

        public virtual List<CheckPointDTO> CheckPoints { get; set; }
        public virtual List<RequirementDTO> Requirements { get; set; }
    }
}
