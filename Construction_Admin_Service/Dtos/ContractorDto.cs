using Construction_Admin_Service.Models;

namespace Construction_Admin_Service.Dtos
{
    public class ContractorDto
    {
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<ContractorQuotation> ContractorQuotations { get; set; }

    }
}
