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

namespace Reports
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


      DataContext _contextStruct;
      _contextStruct = new DataContext(conn.ServerStruct, conn.DataBaseStruct);

      services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(conn.ServiceBusConnectionString, _context);
      IServiceReports serviceReports = new ServiceReports(_context, _contextLog, conn.TokenServer, conn.ServiceBusConnectionString, serviceControlQueue);

      serviceReports.RegisterOnMessageHandlerAndReceiveMesssages();

      services.AddScoped<IServiceElearningFluid>(_ => new ServiceElearningFluid(_context));
      services.AddScoped<IServiceObjective>(_ => new ServiceObjective(_context));
      services.AddScoped<IServiceOffBoarding>(_ => new ServiceOffBoarding(_context));
      services.AddScoped<IServiceOnBoarding>(_ => new ServiceOnBoarding(_context, _context, conn.TokenServer, serviceControlQueue));
      services.AddScoped<IServiceReports>(_ => new ServiceReports(_context, _context, conn.TokenServer, conn.ServiceBusConnectionString, serviceControlQueue));
      services.AddScoped<IServiceControlQueue>(_ => new ServiceControlQueue(conn.ServiceBusConnectionString, _context));
      services.AddScoped<IServiceAccount>(_ => new ServiceAccount(_context, _context, serviceControlQueue));
      services.AddScoped<IServiceAuthentication>(_ => new ServiceAuthentication(_context, _context, serviceControlQueue, conn.SignalRService));
      services.AddScoped<IServicePerson>(_ => new ServicePerson(_context, _context, serviceControlQueue, conn.SignalRService));
      services.AddScoped<IServiceIndicators>(_ => new ServiceIndicators(_context, _context, conn.TokenServer));
      services.AddScoped<IServiceParameters>(_ => new ServiceParameters(_context));

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
      //services.AddSignalR();

      // Configurando o serviço de documentação do Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
              Title = "Reports - Fluid",
              Version = "v1",
              Description = "Sistema de carreiras fluidas",
            });
        string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
        string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
        string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
        c.IncludeXmlComments(caminhoXmlDoc);
      });

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
      //app.UseSignalR(routes =>
      //{
      //  routes.MapHub<MessagesHub>("/messagesHub");
      //});
      // Ativando middlewares para uso do Swagger
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "help";
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Reports");
      });
    }
  }
}
