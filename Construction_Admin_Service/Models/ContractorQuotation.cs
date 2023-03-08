using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction_Admin_Service.Models
{
    public class ContractorQuotation
    {
        [Key]
        public int QuotationID { get; set; }    
        public string QuotatinDetail { get; set; }
        public decimal QuotationAmount { get; set; }

        public decimal AmmountPaid { get; set; }

        public bool Quotationstatus { get; set; }

        public  bool Amountstatus { get; set; }

        //[ForeignKey("ContractorId")]
        public Contractor Contractor { get; set; }

        public int ContractorId { get; set; }
    }
}
