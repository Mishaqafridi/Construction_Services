using Construction_Admin_Service.Dtos;
using Construction_Admin_Service.Models;

namespace Construction_Admin_Service.Services
{
    public interface IContractor
    {

        Task<ServiceResponse<List<ContractorDto>>> GetAllContactor();

        Task<ServiceResponse<List<GetContractorDrop>>> GetContractorDrop();
        Task<ServiceResponse<ContractorDto>> GetContactorById(int id);
        Task<ServiceResponse<int>> AddContactor(AddContractorDto Contractors);
        Task<ServiceResponse<ContractorDto>> UpdateContactor(UpdateContractorDto Contractors);

        Task<ServiceResponse<GetPaymentDto>> GetContactorPaymentById(int id);
        Task<ServiceResponse<int>> AddContactorPayment(AddPaymentDto Contractors);
        Task<ServiceResponse<GetPaymentDto>> UpdateContactorPayment(UpdatePaymentDto Contractors);


    }
}
