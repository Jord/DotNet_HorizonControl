using Microsoft.Extensions.Configuration;

namespace HorizonControlCenterBAL
{
    public static class EndPointsConfiguration
    {
        public static void InitEndPointsListCollection(string environment)
        {
            var configBuilder = new ConfigurationBuilder();

            //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings." + environment + ".json");
            configBuilder.AddJsonFile(path, false);


            //var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings." + environment + ".json");
            //configBuilder.AddJsonFile(path, true);// NOT OPTIONAL HAS TO BE THERE

            var root = configBuilder.Build();

            //            GlobalEndpoints.GlobalServerURL = root.GetSection("GlobalEndPoints:GlobalServerURL").Value.ToString();

            // GlobalEndpoints.GetDivisionsListURL = GlobalEndpoints.GlobalServerURL + root.GetSection("GlobalEndPoints:DivisionsListEndpoint").Value.ToString();

            //            GlobalEndpoints.GetJordSuitesListURL = GlobalEndpoints.GlobalServerURL + root.GetSection("GlobalEndPoints:JordSuitesListEndpoint").Value.ToString();

            // GlobalEndpoints.GetDivisionsForDropdownURL = GlobalEndpoints.GlobalServerURL + root.GetSection("GlobalEndPoints:DivisionsDropdownsListEndpoint").Value.ToString();

            //  GlobalEndpoints.GetJordSuitesForDropdownURL = GlobalEndpoints.GlobalServerURL + root.GetSection("GlobalEndPoints:JordSuitesDropdownsListEndpoint").Value.ToString();


            //var PathStrSetting = root.GetSection("ServerPathDivisionsListEndPoint:ServerPathDivisionsList");

            // string DivisionsPath = PathStrSetting.Value;
            //return DivisionsPath;



            CentralLogEndpoints.CentralLogServerURL = root.GetSection("CentralLogEndPoints:CentralLogServerURL").Value.ToString();

            var distributionEndpoint = root.GetSection("CentralLogEndPoints:DistributionEndpoint").Value?.ToString();
            CentralLogEndpoints.DistributionURL = !string.IsNullOrEmpty(distributionEndpoint)
                ? CentralLogEndpoints.CentralLogServerURL + distributionEndpoint : string.Empty;

            var statusLogEndpoint = root.GetSection("CentralLogEndPoints:StatusLogEndpoint").Value?.ToString();
            CentralLogEndpoints.StatusLogURL = !string.IsNullOrEmpty(statusLogEndpoint)
                ? CentralLogEndpoints.CentralLogServerURL + statusLogEndpoint
                : string.Empty;

            var auditHeaderLogEndpoint = root.GetSection("CentralLogEndPoints:AuditHeaderLogEndpoint").Value?.ToString();
            CentralLogEndpoints.AuditHeaderLogURL = !string.IsNullOrEmpty(auditHeaderLogEndpoint)
                ? CentralLogEndpoints.CentralLogServerURL + auditHeaderLogEndpoint
                : string.Empty;

            //CentralLogEndpoints.DistributionURL = CentralLogEndpoints.CentralLogServerURL + root.GetSection("CentralLogEndPoints:DistributionEndpoint").Value.ToString() ?? string.Empty;

            //CentralLogEndpoints.StatusLogURL = CentralLogEndpoints.CentralLogServerURL + root.GetSection("CentralLogEndPoints:StatusLogEndpoint").Value.ToString() ?? string.Empty;

            //CentralLogEndpoints.AuditHeaderLogURL = CentralLogEndpoints.CentralLogServerURL + root.GetSection("CentralLogEndPoints:AuditHeaderLogEndpoint").Value.ToString() ?? string.Empty; // --added

            //CentralLogEndpoints.AuditDetailLogURL = CentralLogEndpoints.CentralLogServerURL + root.GetSection("CentralLogEndPoints:AuditDetailLogEndpoint").Value.ToString(); // --added


            // AttachmentsEndPoints.AttachmentsServerURL = root.GetSection("AttachmentsEndPoints:AttachmentsServerURL").Value.ToString();

            // AttachmentsEndPoints.FileUploadURL = AttachmentsEndPoints.AttachmentsServerURL + root.GetSection("AttachmentsEndPoints:FileUploadEndpoint").Value.ToString();

            // AttachmentsEndPoints.SaveFileURL = AttachmentsEndPoints.AttachmentsServerURL + root.GetSection("AttachmentsEndPoints:SaveFileEndpoint").Value.ToString();

            // AttachmentsEndPoints.FileUploadWasabi = AttachmentsEndPoints.AttachmentsServerURL + root.GetSection("AttachmentsEndPoints:FileUploadToWasabi").Value.ToString();

            //  PathEndPoint.PathURL = root.GetSection("PathEndPoint:PathURL").Value.ToString();

        }
    }
}
