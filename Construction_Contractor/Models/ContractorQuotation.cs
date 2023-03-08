namespace Construction_Contractor.Models
{
    public class ContractorQuotation
    {

        public int QuotationID { get; set; }    
        public string QuotatinDetail { get; set; }
        public decimal QuotationAmount { get; set; }

        public decimal AmmountPaid { get; set; }

        public bool Quotationstatus { get; set; }

        public  bool Amountstatus { get; set; }

        public Contractor Contractors { get; set; } 
    }
}
