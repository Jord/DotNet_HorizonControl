//using CentralLogModels;
//using HorizonControlCenterDAL.Entities;
//using HorizonControlCenterWebAPI;
//using HorizonControlCenterWebAPI.Security;
//using HorizonControlCenterWebAPI.Services.UserService;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.ApplicationModels;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using SecurityAuthorization;
//using Serilog;
//using Serilog.Events;
//using Serilog.Sinks.PostgreSQL.ColumnWriters;
//using System.Reflection;
//using System.Security.Claims;
//using System.Text;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<horizoncontrolContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("HCCDB")));

//var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


//builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//builder.Configuration.AddJsonFile("appsettings." + environment + ".json", optional: true, reloadOnChange: true);
//builder.Configuration.AddEnvironmentVariables();

//string configAllowSpecificOrigins = "_ConfigAllowSpecificOrigins";
//// Add services to the container.
//builder.Services.AddControllers(options =>
//{
//    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
//});
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

////Notes: HttpContext is not available in controller's constructor by default
//// we need to do it with dependency injectlike below
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddHttpContextAccessor();

//// Register AutoMapper
//builder.Services.RegisterAutoMapper();

//// Register BAL services
//builder.Services.AddScoped<HorizonControlCenterBAL.SuiteBAL>();
//builder.Services.AddScoped<HorizonControlCenterBAL.SuitesApplicationBAL>();

//ConfigVariables configVariables = builder.Configuration.GetSection("ConfigVariables").Get<ConfigVariables>();


//builder.Services.AddSwaggerGen();

//builder.Services.AddCors(c =>
//{
//    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
//});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: configAllowSpecificOrigins,
//                    builder =>
//                    {
//                        builder.WithOrigins("http://localhost:3000", "http://hyddevsrv", "https://dryrun.jord.com.au")
//                                  .AllowAnyHeader()
//                                  .AllowAnyMethod();
//                    });
//});


//var app = builder.Build();


//// Started Adding by KC
//app.UseCors("AllowOrigin");
//app.UseCors(configAllowSpecificOrigins);
//// Ended Adding by KC

//CryptoUtill.key = configVariables.EncKey;
//CryptoUtill.IV = configVariables.EncIV;
//if (environment != "Development")
//{
//    builder.WebHost.UseUrls("http://localhost:5025");
//}

//app.UsePathBase(new PathString("/horizon-control-center"));
//app.UseStaticFiles();
//app.UseRouting();

//app.UseSwagger();
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/horizon-control-center/swagger/v1/swagger.json", "Horizon Control Center Web API");
//    c.DisplayRequestDuration();    // Show request duration
//    c.EnablePersistAuthorization();       // Keep JWT token across refresh
//    c.EnableFilter();               // Add search filter on endpoints
//    c.EnableDeepLinking();        //This will copy complete API url string
//    c.DisplayRequestDuration();
//    c.EnableFilter();
//    c.EnableDeepLinking();
//    c.EnablePersistAuthorization();
//});

//HorizonControlCenterBAL.EndPointsConfiguration.InitEndPointsListCollection(environment);

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

using DotNet.Extensions;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterWebAPI.Extensions;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();

string appName = "HorizonControlCenter";
string dbStringName = "HorizonControlCenterDB";

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

