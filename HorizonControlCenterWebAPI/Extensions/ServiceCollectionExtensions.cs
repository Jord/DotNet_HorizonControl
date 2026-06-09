using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys.Cryptography;
using CentralLogModels;
//using DotNetShared.Services;
using HorizonControlCenterBAL;
using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterDAL;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterWebAPI.Services.UserService;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;



namespace HorizonControlCenterWebAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddAppServices(
   this IServiceCollection services,
   IConfiguration configuration,
   IWebHostEnvironment environment)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDataAccessServices();
            services.AddBusinessLogicServices();

            services.AddScoped<IUserService, UserService>();
            services.AddHttpContextAccessor();

            services.Configure<AuditLogOptions>(
            configuration.GetSection("AuditLogOptions"));

            services.Configure<StatusLogOptions>(
            configuration.GetSection("StatusLogOptions"));


            services.AddAuditLogger(opts =>
            {
                opts.BaseUrl = configuration["Horizon:Services:Audit:BaseUrl"];
                opts.AuditEndpoint = configuration["Horizon:Services:CentralLog:AuditHeaderEndpoint"];
            });

            services.AddStatusLogger(opts =>
            {
                opts.BaseUrl = configuration["Horizon:Services:Audit:BaseUrl"];
                opts.StatusLogEndpoint = configuration["Horizon:Services:CentralLog:StatusLogEndpoint"];
            });

            //services(opts =>
            //{
            //    opts.BaseUrl = configuration["Horizon:Services:Email:BaseUrl"];
            //    opts.SendEndpoint = configuration["Horizon:Services:Email:SendEndpoint"];
            //});
            services.AddApplicationServices();
            return services;
        }

        
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            //services.AddScoped<IUserService, UserService>();
           // services.AddScoped<IAuthUserContext, AuthUserContext>();
            return services;
        }
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<ISuiteBAL, SuiteBAL>();
            services.AddScoped<IGroupBAL, GroupBAL>();
            services.AddScoped<ISuitesApplicationBAL, SuitesApplicationBAL>();
            services.AddScoped<IGroupGroupBAL, GroupGroupBAL>();
            services.AddScoped<IUserBAL, UserBAL>();
            services.AddScoped<IUserGroupBAL, UserGroupBAL>();
            services.AddScoped<IGroupRoleBAL, GroupRoleBAL>();



            return services;
        }

        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<SuiteDAL>();
            services.AddScoped<GroupDAL>();
            services.AddScoped<SuitesApplicationDAL>();
            services.AddScoped<GroupGroupDAL>();
            services.AddScoped<UserDAL>();
            services.AddScoped<UserGroupDAL>();
            services.AddScoped<GroupRoleDAL>();


            services.AddScoped<ISuiteDAL, SuiteDAL>();
            services.AddScoped<IGroupDAL, GroupDAL>();
            services.AddScoped<ISuitesApplicationDAL, SuitesApplicationDAL>();
            services.AddScoped<IGroupGroupDAL, GroupGroupDAL>();
            services.AddScoped<IUserDAL, UserDAL>();
            services.AddScoped<IUserGroupDAL, UserGroupDAL>();
            services.AddScoped<IGroupRoleDAL, GroupRoleDAL>();

            return services;
        }
    }
}

    

