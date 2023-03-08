using System.ComponentModel.DataAnnotations;

namespace Construction_Contractor.Models
{
    public class Contractor
    {
          [Key]
        public int ContractorId { get; set; }    
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

    }
}
