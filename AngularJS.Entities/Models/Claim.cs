using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace AngularJS.Entities.Models
{
    public partial class Claim : Entity
    {
        public Claim()
        {
            this.CheckPoints = new List<CheckPoint>();
            this.Requirements = new List<Requirement>();
        }

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

        // public virtual Request Request { get; set; }
        public virtual ICollection<CheckPoint> CheckPoints { get; set; }
        public virtual ICollection<Requirement> Requirements { get; set; }
    }
}
