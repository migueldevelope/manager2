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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Tools;

namespace ManagerMessages
{
  /// <summary>
  /// Controle de inicialização da API
  /// </summary>
  public class Startup
  {

    /// <summary>
    /// Construtor do controle
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    /// <summary>
    /// Propriedade de configuração
    /// </summary>
    public IConfiguration Configuration { get; }
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

    /// <summary>
    /// Registrador de serviços
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
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
      IServicePerson servicePerson = new ServicePerson(_context, _contextLog, serviceControlQueue, conn.SignalRService);
      IServiceLog serviceLog = new ServiceLog(_context, _contextLog);
      IServiceWorkflow serviceWorkflow = new ServiceWorkflow(_context, _contextLog, serviceControlQueue, conn.SignalRService);
      IServiceAutoManager serviceAutoManager = new ServiceAutoManager(_context, _contextLog, serviceControlQueue, servicePerson,conn.TokenServer);
      IServiceInfra serviceInfra = new ServiceInfra(_context);
      IServiceOnBoarding serviceOnBoarding = new ServiceOnBoarding(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceMonitoring serviceMonitoring = new ServiceMonitoring(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceIndicators serviceIndicators = new ServiceIndicators(_context, _contextLog, conn.TokenServer,servicePerson);
      IServiceMandatoryTraining serviceMandatoryTraining = new ServiceMandatoryTraining(_context);
      IServicePlan servicePlan = new ServicePlan(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceCheckpoint serviceCheckpoint = new ServiceCheckpoint(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceParameters serviceParameters = new ServiceParameters(_context);
      IServiceEvent serviceEvent = new ServiceEvent(_context, _contextLog, conn.TokenServer, conn.BlobKey);
      IServiceUser serviceUser = new ServiceUser(_context, _contextLog);
      IServiceConfigurationNotifications serviceConfigurationNotifications = new ServiceConfigurationNotifications(_context);
      IServiceNotification serviceNotification = new ServiceNotification(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceLogMessages serviceLogMessages = new ServiceLogMessages(_context);


      services.AddSingleton(_ => serviceMaturity);
      services.AddSingleton(_ => serviceUser);
      services.AddSingleton(_ => serviceLogMessages);
      // Start service
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
    /// <summary>
    /// Configurador de servicos
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
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
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://localhost")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("x-total-count")
                    .AllowCredentials());
            });

            //services.AddMvc();
            services.AddRazorPages();

            services.AddControllersWithViews()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSignalR();

      RegistreServices(services);
    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// <summary>
    /// Configuração de aplicação
    /// </summary>
    /// <param name="app">Aplicação</param>
    /// <param name="env">Ambiente de hospedagem</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("x-total-count"));
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
  }
}
