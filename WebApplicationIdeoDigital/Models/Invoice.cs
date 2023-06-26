using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationIdeoDigital.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceID { get; set; }

        public DateTime Date { get; set; }

        public int CustomerID { get; set; }

        public string Status { get; set; }

        public int Amount { get; set; }
    }
    /** InvoiceID Date CustomerID Status Amount **/
}
