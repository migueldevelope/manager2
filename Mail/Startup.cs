using System;
using System.Text;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Data;
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

namespace Mail
{
  /// <summary>
  /// Controlador de inicialização do projeto
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// Propriedade publica do configurador
    /// </summary>
    public IConfiguration Configuration { get; }
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

    /// <summary>
    /// Ponto de entrada do configurador
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    /// <summary>
    /// Configurador de serviços
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    public void RegistreServices(IServiceCollection services)
    {
      DataContext _context;
      var conn = XmlConnection.ReadConfig();
      _context = new DataContext(conn.Server, conn.DataBase);

      DataContext _contextLog;
      _contextLog = new DataContext(conn.ServerLog, conn.DataBaseLog);
      string serviceBusConnectionString = conn.ServiceBusConnectionString;
      string queueName = conn.QueueName;

      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, queueName);

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      IServiceMailMessage serviceMailMessage = new ServiceMailMessage(_contextLog);
      IServiceSendGrid serviceSendGrid = new ServiceSendGrid(_contextLog);
      IServiceMailModel serviceMailModel = new ServiceMailModel(_context);

      services.AddSingleton(_ => serviceMailMessage);
      services.AddSingleton(_ => serviceSendGrid);
      services.AddSingleton(_ => serviceMailModel);
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
        options.AddPolicy("AllowAll",
          builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
      ));
      services.AddMvc();
      RegistreServices(services);
    }
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// <summary>
    /// Configurador de aplicação
    /// </summary>
    /// <param name="app">Aplicação</param>
    /// <param name="env">Ambiente de hospedagem</param>
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
