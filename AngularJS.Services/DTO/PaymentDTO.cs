using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.DTO
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public Int32? ClaimID { get; set; }
        public string InvoiceCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Decimal? ExchangeRate { get; set; }
        public Decimal? VendorPayment { get; set; }
        public string NoteCalculate { get; set; }
        public string Note { get; set; }
    }
}
