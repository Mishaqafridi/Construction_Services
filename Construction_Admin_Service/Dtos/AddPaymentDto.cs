using Construction_Admin_Service.Models;

namespace Construction_Admin_Service.Dtos
{
    public class AddPaymentDto
    {
        public string QuotatinDetail { get; set; }
        public decimal QuotationAmount { get; set; }

        public decimal AmmountPaid { get; set; }
        public Contractor Contractors { get; set; }
        public int ContractorId { get; set; }
        public bool Quotationstatus { get; set; }

        public bool Amountstatus { get; set; }
    }
}
