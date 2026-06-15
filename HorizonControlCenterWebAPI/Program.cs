using DotNet.Extensions;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterWebAPI.Extensions;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();

string appName = "HorizonControlCenter";
string dbStringName = "HorizonControlDB";

builder.ConfigureBuilder(environment, appName, "https://dotnethorizonappconfig.azconfig.io");

DotNet.Extensions.EndPointsConfiguration.InitEndPointsListCollection(builder.Configuration);
//var connString = builder.Configuration.GetConnectionString(dbStringName);

//Console.WriteLine($"DB String Name: {dbStringName}");
//Console.WriteLine($"Connection Found: {!string.IsNullOrEmpty(connString)}");

//string GetConnection(string name)

//{

//    return builder.Configuration[$"Horizon:ConnectionStrings:{name}"]

//        ?? builder.Configuration.GetConnectionString(name)

//        ?? throw new Exception($"{name} connection string not found");

//}

//var connString = GetConnection(dbStringName);

builder.Services.AddCoreServices<horizoncontrolContext>(
    builder.Configuration,
     ODataExtensions.BuildEdmModel,
    environment,
    dbStringName,
    appName
);

builder.Services.AddAppServices(builder.Configuration, builder.Environment);

builder.Build().SetWebApp(builder.Configuration, environment, appName, "horizon-control-service");

