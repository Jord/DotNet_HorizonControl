using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using HorizonControlCenterModels.Enums;
using System;

namespace HorizonControlCenterDAL.Mapping
{
    public class SuitesApplicationProfile : Profile
    {
        public SuitesApplicationProfile()
        {
            CreateMap<SuitesApplicationDTO, SuitesApplication>()
                .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => 
                    ParseApplicationType(src.ApplicationType)));

            CreateMap<SuitesApplication, SuitesApplicationModel>()
                .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.ApplicationType));

            CreateMap<SuitesApplicationModel, SuitesApplication>()
                .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.ApplicationType));
        }

        private static int? ParseApplicationType(string? applicationType)
        {
            if (string.IsNullOrWhiteSpace(applicationType))
                return null;

            if (Enum.TryParse<ApplicationType>(applicationType, true, out var appType))
                return (int)appType;

            return null;
        }
    }
}
