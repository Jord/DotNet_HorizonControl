using AutoMapper;
using System.Reflection;

namespace HorizonControlCenterWebAPI
{
    public static class AutoMapperConfig
    {
        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            // Get assemblies that contain AutoMapper profiles
            var assemblies = new[]
            {
                Assembly.GetExecutingAssembly(), // WebAPI
                Assembly.GetAssembly(typeof(HorizonControlCenterDAL.Entities.Suite)), // DAL
                Assembly.GetAssembly(typeof(HorizonControlCenterModels.SuiteModel)), // Models
                Assembly.GetAssembly(typeof(HorizonControlCenterBAL.SuiteBAL)) // BAL
            }.Where(a => a != null).ToArray();

            services.AddAutoMapper(cfg => { }, assemblies!);
        }
    }
}
