using AutoMapper;
using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using HorizonControlCenterModels.Enums;
using System;

namespace HorizonControlCenterDAL.Mapping.Profiles
{
    public class SuitesApplicationProfile : Profile
    {
        public SuitesApplicationProfile()
        {
           // DTO(string) ? Entity(string)

           CreateMap<SuitesApplicationDTO, SuiteApplication>()
               .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src =>
                   ParseApplicationType(src.ApplicationType, src.ApplicationName)));

            // Entity (string) ? Model (string)
            CreateMap<SuiteApplication, SuitesApplicationModel>()
                .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.ApplicationType));

            // Model (string) ? Entity (string)
            CreateMap<SuitesApplicationModel, SuiteApplication>()
                .ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.ApplicationType));
        }

        private static string? ParseApplicationType(string? applicationType, string? applicationName)
        {
            // Priority 1: If ApplicationType is explicitly provided, normalize it
            if (!string.IsNullOrWhiteSpace(applicationType))
            {
                var normalized = ApplicationType.Normalize(applicationType);
                if (normalized != null)
                    return normalized;
            }

            // Priority 2: Infer from ApplicationName if ApplicationType is null or invalid
            if (!string.IsNullOrWhiteSpace(applicationName))
            {
                var inferred = ApplicationType.InferFromName(applicationName);
                if (inferred != null)
                    return inferred;
            }

            // Default: Return null if no match found
            return null;
        }
    }
}
