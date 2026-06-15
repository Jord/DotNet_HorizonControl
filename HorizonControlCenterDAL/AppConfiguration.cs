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
        private static DbContextOptionsBuilder<horizoncontrolContext>? opsBuilder { get; set; }
        private static DbContextOptions<horizoncontrolContext>? dbOptions { get; set; }
        public static DbContextOptions<horizoncontrolContext> ngsqlConnectionOptions()
        {
            var configBuilder = new ConfigurationBuilder();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var defpath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(defpath, false);// NOT OPTIONAL HAS TO BE THERE

            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings." + environment + ".json");
            configBuilder.AddJsonFile(path, true);// NOT OPTIONAL HAS TO BE THERE

            var root = configBuilder.Build();
            var conStrSetting = root.GetSection("ConnectionStrings:HORIZOCONTROLDB");
            opsBuilder = new DbContextOptionsBuilder<horizoncontrolContext>();
            opsBuilder.UseNpgsql(conStrSetting.Value);
            dbOptions = opsBuilder.Options;

            return dbOptions;
        }
    }
}
