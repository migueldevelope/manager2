using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Data;
using Manager.Data.Infrastructure;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Tools;

namespace Manager
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
      new MigrationHandle(_context._db).Migrate();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(conn.ServiceBusConnectionString, _context);
      IServiceBaseHelp serviceBaseHelp = new ServiceBaseHelp(_context, conn.ServiceBusConnectionString);
      IServiceOnBoarding serviceOnBoarding = new ServiceOnBoarding(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      serviceControlQueue.RegisterOnMessageHandlerAndReceiveMesssages();
      //audios
      serviceOnBoarding.RegisterOnMessageHandlerAndReceiveMesssages();
      serviceBaseHelp.RegisterOnMessageHandlerAndReceiveMesssages();

      services.AddScoped<IServiceElearningFluid>(_ => new ServiceElearningFluid(_context));
      services.AddScoped<IServiceObjective>(_ => new ServiceObjective(_context));
      services.AddScoped<IServiceFeelingDay>(_ => new ServiceFeelingDay(_context));
      services.AddScoped<IServiceNewsletter>(_ => new ServiceNewsletter(_context));
      services.AddScoped<IServiceOffBoarding>(_ => new ServiceOffBoarding(_context));
      services.AddScoped<IServiceHRDrive>(_ => new ServiceHRDrive(_context));
      services.AddScoped<IServiceMyAwareness>(_ => new ServiceMyAwareness(_context));
      services.AddScoped<IServiceFluidCareers>(_ => new ServiceFluidCareers(_context));
      services.AddScoped<IServiceRecommendation>(_ => new ServiceRecommendation(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServiceBaseHelp>(_ => new ServiceBaseHelp(_context, conn.TokenServer));
      services.AddScoped<IServiceMaturity>(_ => new ServiceMaturity(_context));
      services.AddScoped<IServiceControlQueue>(_ => new ServiceControlQueue(conn.ServiceBusConnectionString, _context));
      services.AddScoped<IServiceMeritocracy>(_ => new ServiceMeritocracy(_context, _context));
      services.AddScoped<IServiceTermsOfService>(_ => new ServiceTermsOfService(_context));
      //services.AddScoped<IServiceGoals>(_ => new ServiceGoals(_context,));
      services.AddScoped<IServiceCertification>(_ => new ServiceCertification(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServiceUser>(_ => new ServiceUser(_context, _context));
      services.AddScoped<IServiceDictionarySystem>(_ => new ServiceDictionarySystem(_context));
      services.AddScoped<IServiceSalaryScale>(_ => new ServiceSalaryScale(_context));
      services.AddScoped<IServiceLogMessages>(_ => new ServiceLogMessages(_context));
      services.AddScoped<IServiceConfigurationNotifications>(_ => new ServiceConfigurationNotifications(_context));

      services.AddScoped<IServiceAccount>(_ => new ServiceAccount(_context, _context, serviceControlQueue));
      services.AddScoped<IServiceCompany>(_ => new ServiceCompany(_context));
      services.AddScoped<IServiceAuthentication>(_ => new ServiceAuthentication(_context, _context, serviceControlQueue, conn.SignalRService));
      services.AddScoped<IServicePerson>(_ => new ServicePerson(_context, _context, serviceControlQueue, conn.SignalRService));
      services.AddScoped<IServiceWorkflow>(_ => new ServiceWorkflow(_context, _context, serviceControlQueue, conn.SignalRService));
      services.AddScoped<IServiceAutoManager>(_ => new ServiceAutoManager(_context, _context, serviceControlQueue, conn.TokenServer));
      services.AddScoped<IServiceLog>(_ => new ServiceLog(_context, _context));
      services.AddScoped<IServiceInfra>(_ => new ServiceInfra(_context));
      services.AddScoped<IServiceOnBoarding>(_ => new ServiceOnBoarding(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServiceMonitoring>(_ => new ServiceMonitoring(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServicePlan>(_ => new ServicePlan(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServiceIndicators>(_ => new ServiceIndicators(_context, _context, conn.TokenServer));
      services.AddScoped<IServiceCheckpoint>(_ => new ServiceCheckpoint(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServiceParameters>(_ => new ServiceParameters(_context));

      //serviceIndicators.SendMessages(conn.SignalRService);

      //Task.Run(() => CallAPIColdStart(serviceAuthentication, conn.TokenServer));

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
                  builder => builder.WithOrigins("http://localhosts/*", "10.0.0.16", "https://test.analisa.solutions/*")
                  //builder => builder.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .WithExposedHeaders("x-total-count")
                  .AllowCredentials());
      });

      //services.AddMvc();
      services.AddRazorPages();

      services.AddControllersWithViews()
      .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

      //services.AddSignalR();

      // Configurando o serviço de documentação do Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
              Title = "Manager - Fluid",
              Version = "v1",
              Description = "Sistema de carreiras fluidas"
            });
        string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
        string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
        string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
        c.IncludeXmlComments(caminhoXmlDoc);
      });

      //      Configuration.GetConnectionString("10.0.0.16,port: 6379,password=bti9010");

      //services.AddMemoryCache();
      //services.AddDistributedRedisCache(options =>
      //{
      //  options.Configuration =
      //      Configuration.GetConnectionString("redis://10.0.0.16:6379?password=bti9010");

      //  options.InstanceName = "analisa";
      //});

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
      app.UseRouting();
      app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("x-total-count"));
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDefaultControllerRoute();
        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
      });
      //app.UseSignalR(routes =>
      //{
      //  routes.MapHub<MessagesHub>("/MessagesHub");
      //});
      // Ativando middlewares para uso do Swagger
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "help";
        //c.SwaggerEndpoint("..swagger/v1/swagger.json", "Manager");
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Manager");
      });
    }
  }
}
