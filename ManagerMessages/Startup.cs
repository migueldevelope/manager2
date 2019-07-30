using System;
using System.Text;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tools;

namespace ManagerMessages
{
  public class Startup
  {

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    public IConfiguration Configuration { get; }
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

    public void RegistreServices(IServiceCollection services)
    {
      DataContext _context;
      var conn = XmlConnection.ReadVariablesSystem();
      _context = new DataContext(conn.Server, conn.DataBase);

      DataContext _contextLog;
      _contextLog = new DataContext(conn.ServerLog, conn.DataBaseLog);
      string serviceBusConnectionString = conn.ServiceBusConnectionString;

      IServiceMaturity serviceMaturity = new ServiceMaturity(_context);
      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, serviceMaturity);
      serviceControlQueue.StartMathMaturity();

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      IServiceAccount serviceAccount = new ServiceAccount(_context, _contextLog, serviceControlQueue);
      IServiceCompany serviceCompany = new ServiceCompany(_context);
      IServicePerson servicePerson = new ServicePerson(_context, _contextLog, serviceControlQueue);
      IServiceLog serviceLog = new ServiceLog(_context, _contextLog);
      IServiceWorkflow serviceWorkflow = new ServiceWorkflow(_context, _contextLog, serviceControlQueue);
      IServiceAutoManager serviceAutoManager = new ServiceAutoManager(_context, _contextLog, serviceControlQueue);
      IServiceInfra serviceInfra = new ServiceInfra(_context);
      IServiceOnBoarding serviceOnBoarding = new ServiceOnBoarding(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceMonitoring serviceMonitoring = new ServiceMonitoring(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceIndicators serviceIndicators = new ServiceIndicators(_context, _contextLog, conn.TokenServer);
      IServiceMandatoryTraining serviceMandatoryTraining = new ServiceMandatoryTraining(_context);
      IServicePlan servicePlan = new ServicePlan(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceCheckpoint serviceCheckpoint = new ServiceCheckpoint(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceParameters serviceParameters = new ServiceParameters(_context);
      IServiceEvent serviceEvent = new ServiceEvent(_context, _contextLog, conn.TokenServer);
      IServiceUser serviceUser = new ServiceUser(_context, _contextLog);
      IServiceConfigurationNotifications serviceConfigurationNotifications = new ServiceConfigurationNotifications(_context);
      IServiceNotification serviceNotification = new ServiceNotification(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceLogMessages serviceLogMessages = new ServiceLogMessages(_context);


      services.AddSingleton(_ => serviceMaturity);
      services.AddSingleton(_ => serviceUser);
      services.AddSingleton(_ => serviceLogMessages);
      /// Start service
      serviceNotification.SendMessage();
      services.AddSingleton(_ => serviceNotification);
      services.AddSingleton(_ => serviceConfigurationNotifications);
      services.AddSingleton(_ => serviceAccount);
      services.AddSingleton(_ => serviceCompany);
      services.AddSingleton(_ => servicePerson);
      services.AddSingleton(_ => serviceWorkflow);
      services.AddSingleton(_ => serviceAutoManager);
      services.AddSingleton(_ => serviceLog);
      services.AddSingleton(_ => serviceInfra);
      services.AddSingleton(_ => serviceOnBoarding);
      services.AddSingleton(_ => serviceMonitoring);
      services.AddSingleton(_ => servicePlan);
      services.AddSingleton(_ => serviceIndicators);
      services.AddSingleton(_ => serviceCheckpoint);
      services.AddSingleton(_ => serviceParameters);
      services.AddSingleton(_ => serviceEvent);
      services.AddSingleton(_ => serviceMandatoryTraining);
      serviceIndicators.SendMessages(conn.SignalRService);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
        };

        options.Events = new JwtBearerEvents
        {
          OnAuthenticationFailed = context =>
          {
            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
            return Task.CompletedTask;
          },
          OnTokenValidated = context =>
          {
            Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
            return Task.CompletedTask;
          }
        };
      });
      services.AddCors(options =>
        options.AddPolicy("AllowAll",
          builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
      ));

      services.AddMvc();

      services.AddSignalR();

      RegistreServices(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();
      app.UseAuthentication();
      app.UseCors("AllowAll");
      app.UseMvc();
    }
  }
}
