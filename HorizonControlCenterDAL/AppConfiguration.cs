using HorizonControlCenterDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL
{
    public class AppConfiguration
    {
        private static DbContextOptionsBuilder<horizoncontrolContext>? OpsBuilder { get; set; }
        private static DbContextOptions<horizoncontrolContext>? DbOptions { get; set; }
        
        public static DbContextOptions<horizoncontrolContext> NgsqlConnectionOptions(IConfiguration configuration)
        {
            var conStrSetting = configuration.GetConnectionString("HorizonControlDB")
                ?? configuration["Horizon:ConnectionStrings:HorizonControlDB"];

            if (string.IsNullOrEmpty(conStrSetting))
            {
                throw new InvalidOperationException(
                    "The ConnectionString property has not been initialized. " +
                    "Ensure 'ConnectionStrings:HorizonControlDB' or 'Horizon:ConnectionStrings:HorizonControlDB' is configured in appsettings.");
            }

            OpsBuilder = new DbContextOptionsBuilder<horizoncontrolContext>();
            OpsBuilder.UseNpgsql(conStrSetting);
            DbOptions = OpsBuilder.Options;

            return DbOptions;
        }
    }
}
