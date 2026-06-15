using DotNet.Extensions;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterWebAPI.Extensions;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();

string appName = "HorizonControlCenter";
string dbStringName = "HorizonControlDB";

builder.ConfigureBuilder(environment, appName, "https://dotnethorizonappconfig.azconfig.io");

DotNet.Extensions.EndPointsConfiguration.InitEndPointsListCollection(builder.Configuration);

builder.Services.AddCoreServices<horizoncontrolContext>(
    builder.Configuration,
     ODataExtensions.BuildEdmModel,
    environment,
    dbStringName,
    appName
);

builder.Services.AddAppServices(builder.Configuration, builder.Environment);

builder.Build().SetWebApp(builder.Configuration, environment, appName, "horizon-control-service");

