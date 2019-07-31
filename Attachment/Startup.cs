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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Tools;

namespace Attachment
{
  /// <summary>
  /// 
  /// </summary>
  public class Startup
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    /// <summary>
    /// 
    /// </summary>
    public IConfiguration Configuration { get; }
    private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
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

      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      IServiceAccount serviceAccount = new ServiceAccount(_context, _contextLog, serviceControlQueue);
      IServiceCompany serviceCompany = new ServiceCompany(_context);
      IServicePerson servicePerson = new ServicePerson(_context, _contextLog, serviceControlQueue);
      IServiceLog serviceLog = new ServiceLog(_context, _contextLog);
      IServiceWorkflow serviceWorkflow = new ServiceWorkflow(_context, _contextLog, serviceControlQueue);
      IServiceAutoManager serviceAutoManager = new ServiceAutoManager(_context, _contextLog, serviceControlQueue, servicePerson);
      IServiceInfra serviceInfra = new ServiceInfra(_context);
      IServiceOnBoarding serviceOnBoarding = new ServiceOnBoarding(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceMonitoring serviceMonitoring = new ServiceMonitoring(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceMandatoryTraining serviceMandatoryTraining = new ServiceMandatoryTraining(_context);
      IServicePlan servicePlan = new ServicePlan(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceEvent serviceEvent = new ServiceEvent(_context, _contextLog, conn.TokenServer);
      IServiceUser serviceUser = new ServiceUser(_context, _contextLog);
      IServiceBaseHelp serviceBaseHelp = new ServiceBaseHelp(_context, serviceBusConnectionString);
      IServiceCertification serviceCertification = new ServiceCertification(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceRecommendation serviceRecommendation = new ServiceRecommendation(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceSalaryScale serviceSalaryScale = new ServiceSalaryScale(_context);

      services.AddSingleton(_ => serviceSalaryScale);
      services.AddSingleton(_ => serviceRecommendation);
      services.AddSingleton(_ => serviceBaseHelp);
      services.AddSingleton(_ => serviceMaturity);
      services.AddSingleton(_ => serviceCertification);
      services.AddSingleton(_ => serviceUser);
      services.AddSingleton(_ => serviceMandatoryTraining);
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
      services.AddSingleton(_ => serviceEvent);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
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
      // Configurando o serviço de documentação do Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new Info
            {
              Title = "Attachment - Analisa Fluid Careers",
              Version = "v1",
              Description = "Sistema de carreiras fluidas",
              Contact = new Contact
              {
                Name = "Jm Soft Informática Ltda",
                Url = "http://www.jmsoft.com.br"
              }
            });
        string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
        string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
        string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
        c.IncludeXmlComments(caminhoXmlDoc);
      });

      RegistreServices(services);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();
      app.UseAuthentication();
      app.UseCors("AllowAll");
      app.UseMvc();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "help";
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Attachment");
      });
    }
  }
}
