using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Manager.Core.Interfaces;
using Manager.Core.Views;
using Manager.Data;
using Manager.Data.Infrastructure;
using Manager.Services.Auth;
using Manager.Services.Commons;
using Manager.Services.Specific;
using Manager.Views.BusinessList;
using Manager.Views.Enumns;
using Manager.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json;
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


      string serviceBusConnectionString = conn.ServiceBusConnectionString;

      new MigrationHandle(_context._db).Migrate();


      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      IServiceMaturity serviceMaturity = new ServiceMaturity(_context);
      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, serviceMaturity);
      IServiceBaseHelp serviceBaseHelp = new ServiceBaseHelp(_context, serviceBusConnectionString);

      IServiceAccount serviceAccount = new ServiceAccount(_context, _contextLog, serviceControlQueue);
      IServiceCompany serviceCompany = new ServiceCompany(_context);
      IServicePerson servicePerson = new ServicePerson(_context, _contextLog, serviceControlQueue, conn.SignalRService);
      IServiceAutoManager serviceAutoManager = new ServiceAutoManager(_context, _contextLog, serviceControlQueue, servicePerson, conn.TokenServer);
      IServiceLog serviceLog = new ServiceLog(_context, _contextLog);
      IServiceWorkflow serviceWorkflow = new ServiceWorkflow(_context, _contextLog, serviceControlQueue, conn.SignalRService);
      
      IServiceInfra serviceInfra = new ServiceInfra(_context);
      IServiceOnBoarding serviceOnBoarding = new ServiceOnBoarding(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceMonitoring serviceMonitoring = new ServiceMonitoring(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceIndicators serviceIndicators = new ServiceIndicators(_context, _contextLog, conn.TokenServer, servicePerson);
      IServiceMandatoryTraining serviceMandatoryTraining = new ServiceMandatoryTraining(_context);
      IServicePlan servicePlan = new ServicePlan(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceCheckpoint serviceCheckpoint = new ServiceCheckpoint(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceParameters serviceParameters = new ServiceParameters(_context);
      IServiceEvent serviceEvent = new ServiceEvent(_context, _contextLog, conn.TokenServer);
      IServiceConfigurationNotifications serviceConfigurationNotifications = new ServiceConfigurationNotifications(_context);
      IServiceLogMessages serviceLogMessages = new ServiceLogMessages(_context);
      IServiceSalaryScale serviceSalaryScale = new ServiceSalaryScale(_context);
      IServiceDictionarySystem serviceDictionarySystem = new ServiceDictionarySystem(_context);
      IServiceUser serviceUser = new ServiceUser(_context, _contextLog);
      IServiceAuthentication serviceAuthentication = new ServiceAuthentication(_context, _contextLog, serviceControlQueue, conn.SignalRService);
      IServiceCertification serviceCertification = new ServiceCertification(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceGoals serviceGoals = new ServiceGoals(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceTermsOfService serviceTermsOfService = new ServiceTermsOfService(_context);
      IServiceRecommendation serviceRecommendation = new ServiceRecommendation(_context, _contextLog, conn.TokenServer, serviceControlQueue);
      IServiceMeritocracy serviceMeritocracy = new ServiceMeritocracy(_context, _contextLog);

      serviceControlQueue.RegisterOnMessageHandlerAndReceiveMesssages();
      serviceBaseHelp.RegisterOnMessageHandlerAndReceiveMesssages();

      services.AddSingleton(_ => serviceRecommendation);
      services.AddSingleton(_ => serviceBaseHelp);
      services.AddSingleton(_ => serviceMaturity);
      services.AddSingleton(_ => serviceControlQueue);
      services.AddSingleton(_ => serviceMeritocracy);
      services.AddSingleton(_ => serviceTermsOfService);
      services.AddSingleton(_ => serviceGoals);
      services.AddSingleton(_ => serviceCertification);
      services.AddSingleton(_ => serviceUser);
      services.AddSingleton(_ => serviceDictionarySystem);
      services.AddSingleton(_ => serviceSalaryScale);
      services.AddSingleton(_ => serviceLogMessages);
      services.AddSingleton(_ => serviceConfigurationNotifications);
      services.AddSingleton(_ => serviceAccount);
      services.AddSingleton(_ => serviceCompany);
      services.AddSingleton(_ => serviceAuthentication);
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
        options.AddPolicy("AllowAll",
          builder => builder
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
          .WithExposedHeaders("x-total-count")
      ));
      services.AddMvc();

      services.AddSignalR();

      // Configurando o serviço de documentação do Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new Info
            {
              Title = "Manager - Analisa Fluid Careers",
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


    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// <summary>
    /// Configuração de aplicação
    /// </summary>
    /// <param name="app">Aplicação</param>
    /// <param name="env">Ambiente de hospedagem</param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();
      app.UseAuthentication();
      app.UseMiddleware<ErrorHandlingMiddleware>();
      app.UseCors("AllowAll");
      app.UseMvc();
      app.UseSignalR(routes =>
      {
        routes.MapHub<MessagesHub>("/MessagesHub");
      });
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
