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
            Mapper.CreateMap<Claim, ClaimDTO>();
            Mapper.CreateMap<Claim, ClaimLiteDTO>();
            Mapper.CreateMap<CheckPoint, CheckPointDTO>();
            Mapper.CreateMap<Requirement, RequirementDTO>();
            // Mapper.CreateMap<Feed, FeedDTO>();
        }
    }
}
