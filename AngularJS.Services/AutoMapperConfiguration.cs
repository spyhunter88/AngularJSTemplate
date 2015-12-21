using System;
using AngularJS.Entities.Models;
using AngularJS.Services.DTO;
using AutoMapper;

namespace AngularJS.Services
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
			Mapper.CreateMap<MenuItem, MenuItemDTO>().ReverseMap();
			
            Mapper.CreateMap<Claim, ClaimDTO>()
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.ClaimDocuments))
                .ReverseMap()
                    .ForMember(dest => dest.ClaimDocuments, opt => opt.MapFrom(src => src.Documents));
            Mapper.CreateMap<Claim, ClaimLiteDTO>();
            Mapper.CreateMap<CheckPoint, CheckPointDTO>().ReverseMap();
            Mapper.CreateMap<Requirement, RequirementDTO>().ReverseMap();
            Mapper.CreateMap<Payment, PaymentDTO>().ReverseMap();
            Mapper.CreateMap<Allocation, AllocationDTO>().ReverseMap();

            Mapper.CreateMap<Document, DocumentDTO>().ReverseMap();
            Mapper.CreateMap<ClaimDocument, DocumentDTO>().ReverseMap();
            // Mapper.CreateMap<Feed, FeedDTO>();
        }
    }
}
