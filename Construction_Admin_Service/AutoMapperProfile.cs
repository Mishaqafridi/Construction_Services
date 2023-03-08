using AutoMapper;
using Construction_Admin_Service.Dtos;
using Construction_Admin_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construction_Admin_Service
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {

            CreateMap<Nationality, GetNationalityDto>();    
            CreateMap<Contractor, GetContractorDrop>();
            CreateMap<Contractor, ContractorDto>();
            CreateMap<AddContractorDto, Contractor>();

            CreateMap<ContractorQuotation, GetPaymentDto>();
            CreateMap<AddPaymentDto, ContractorQuotation>();

        }
    }
}
