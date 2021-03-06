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

namespace Manager
{
  /// <summary>
  /// Controle de inicializa��o da API
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
    /// Propriedade de configura��o
    /// </summary>
    public IConfiguration Configuration { get; }
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";


    /// <summary>
    /// Registrador de servi�os
    /// </summary>
    /// <param name="services">Cole��o de servi�os</param>
    public void RegistreServices(IServiceCollection services)
    {
      DataContext _context;
      var conn = XmlConnection.ReadVariablesSystem();
      _context = new DataContext(conn.Server, conn.DataBase);

      DataContext _contextLog;
      _contextLog = new DataContext(conn.ServerLog, conn.DataBaseLog);

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(conn.ServiceBusConnectionString, _context);

      services.AddScoped<IServiceObjective>(_ => new ServiceObjective(_context));
      services.AddScoped<IServiceMaturity>(_ => new ServiceMaturity(_context));
      services.AddScoped<IServiceControlQueue>(_ => new ServiceControlQueue(conn.ServiceBusConnectionString, _context));
      services.AddScoped<IServiceUser>(_ => new ServiceUser(_context, _context));
      services.AddScoped<IServiceAuthentication>(_ => new ServiceAuthentication(_context, _context, serviceControlQueue, conn.SignalRService));
      services.AddScoped<IServiceLog>(_ => new ServiceLog(_context, _context));
      services.AddScoped<IServiceParameters>(_ => new ServiceParameters(_context));
    }


    // This method gets called by the runtime. Use this method to add services to the container.
    /// <summary>
    /// Configurador de servicos
    /// </summary>
    /// <param name="services">Cole��o de servi�os</param>
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

      // Configurando o servi�o de documenta��o do Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
              Title = "Objective - Fluid",
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
    /// Configura��o de aplica��o
    /// </summary>
    /// <param name="app">Aplica��o</param>
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
