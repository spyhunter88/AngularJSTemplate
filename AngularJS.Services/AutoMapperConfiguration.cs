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
            Mapper.CreateMap<Claim, ClaimDTO>().ReverseMap();
            Mapper.CreateMap<Claim, ClaimLiteDTO>();
            Mapper.CreateMap<CheckPoint, CheckPointDTO>().ReverseMap();
            Mapper.CreateMap<Requirement, RequirementDTO>().ReverseMap();
            Mapper.CreateMap<Payment, PaymentDTO>().ReverseMap();
            Mapper.CreateMap<Allocation, AllocationDTO>().ReverseMap();

            Mapper.CreateMap<Document, DocumentDTO>().ReverseMap();
            // Mapper.CreateMap<Feed, FeedDTO>();
        }
    }
}
