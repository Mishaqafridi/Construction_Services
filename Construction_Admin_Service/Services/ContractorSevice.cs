using AutoMapper;
using Construction_Admin_Service.Data;
using Construction_Admin_Service.Dtos;
using Construction_Admin_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Construction_Admin_Service.Services
{
    public class ContractorSevice : IContractor
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ContractorSevice(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<List<ContractorDto>>> GetAllContactor()
        {
            var serviceresponse = new ServiceResponse<List<ContractorDto>>();
            var dbEmployee = await _context.Contractors.Include(c => c.ContractorQuotations).ToListAsync();
            if (dbEmployee != null)
            {
                serviceresponse.Data = dbEmployee.Select(x => _mapper.Map<ContractorDto>(x)).ToList();
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Contractor record not found";
            }
            return serviceresponse;
        }





        public async Task<ServiceResponse<ContractorDto>> GetContactorById(int id)
        {
            var serviceresponse = new ServiceResponse<ContractorDto>();
            var dbEmployee = await _context.Contractors.Include(c => c.ContractorQuotations).FirstOrDefaultAsync(x => x.ContractorId == id);
            if (dbEmployee != null)
            {
                serviceresponse.Data = _mapper.Map<ContractorDto>(dbEmployee);
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Contractor record not found";
            }
            return serviceresponse;
        }

        public async Task<ServiceResponse<int>> AddContactor(AddContractorDto Contractor)
        {
            ServiceResponse<int> serviceresponse = new ServiceResponse<int>();
            Contractor contractorss = _mapper.Map<Contractor>(Contractor);
            _context.Contractors.Add(contractorss);
            await _context.SaveChangesAsync();
            serviceresponse.Data = contractorss.ContractorId;
            serviceresponse.Message = "Contractor Added Successfully";
            return serviceresponse;
        }


        public async Task<ServiceResponse<List<GetContractorDrop>>> GetContractorDrop()
        {
            var serviceresponse = new ServiceResponse<List<GetContractorDrop>>();
            var dbnationalities = await _context.Contractors.ToListAsync();
            if (dbnationalities != null)
            {
                serviceresponse.Data = dbnationalities.Select(x => _mapper.Map<GetContractorDrop>(x)).ToList();

            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Data not found";
            }
            return serviceresponse;

        }
        public async Task<ServiceResponse<ContractorDto>> UpdateContactor(UpdateContractorDto Contractors)
        {
            var serviceresponse = new ServiceResponse<ContractorDto>();

            //if (GetUser() != null)
            //{
            try
            {
                Contractor dbEmployee = await _context.Contractors.FirstOrDefaultAsync(x => x.ContractorId == Contractors.ContractorId);
                if (dbEmployee != null)
                {
                    dbEmployee.ContractorName = Contractors.ContractorName;
                    dbEmployee.ContractorAddress = Contractors.ContractorAddress;
                    dbEmployee.Phone = Contractors.Phone;
                    dbEmployee.Email = Contractors.Email;
                    await _context.SaveChangesAsync();
                    serviceresponse.Message = "Contractor record updated";
                }
                else
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = "Contractor record not updated";
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;
                return serviceresponse;
            }
            // }
            return serviceresponse;

        }




        public async Task<ServiceResponse<GetPaymentDto>> GetContactorPaymentById(int id)
        {
            var serviceresponse = new ServiceResponse<GetPaymentDto>();
            var dbEmployee = await _context.ContractorQuotations.FirstOrDefaultAsync(x => x.QuotationID == id);
            if (dbEmployee != null)
            {
                serviceresponse.Data = _mapper.Map<GetPaymentDto>(dbEmployee);
            }
            else
            {
                serviceresponse.Success = false;
                serviceresponse.Message = "Contractor record not found";
            }
            return serviceresponse;
        }

        public async Task<ServiceResponse<int>> AddContactorPayment(AddPaymentDto Contractor)
        {
            ServiceResponse<int> serviceresponse = new ServiceResponse<int>();
            ContractorQuotation contractorss = _mapper.Map<ContractorQuotation>(Contractor);
            _context.ContractorQuotations.Add(contractorss);
            await _context.SaveChangesAsync();
            serviceresponse.Data = contractorss.QuotationID;
            serviceresponse.Message = "Quotation Added Successfully";
            return serviceresponse;
        }

        public async Task<ServiceResponse<GetPaymentDto>> UpdateContactorPayment(UpdatePaymentDto Contractors)
        {
            var serviceresponse = new ServiceResponse<GetPaymentDto>();

            //if (GetUser() != null)
            //{
            try
            {
                ContractorQuotation dbEmployee = await _context.ContractorQuotations.FirstOrDefaultAsync(x => x.QuotationID == Contractors.QuotationID);
                if (dbEmployee != null)
                {
                    dbEmployee.QuotatinDetail = Contractors.QuotatinDetail;
                    dbEmployee.QuotationAmount = Contractors.QuotationAmount;
                    dbEmployee.AmmountPaid = Contractors.AmmountPaid;
                    dbEmployee.ContractorId = Contractors.ContractorId;
                    await _context.SaveChangesAsync();
                    serviceresponse.Message = "Contractor Quotation updated";
                }
                else
                {
                    serviceresponse.Success = false;
                    serviceresponse.Message = "Contractor Quotation not updated";
                }
            }
            catch (Exception ex)
            {
                serviceresponse.Success = false;
                serviceresponse.Message = ex.Message;
                return serviceresponse;
            }
            // }
            return serviceresponse;

        }

    }
}
