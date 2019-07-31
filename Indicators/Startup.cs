﻿using System;
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

namespace Indicators
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

      string serviceBusConnectionString = conn.ServiceBusConnectionString;

      //new MigrationHandle(_context._db).Migrate();


      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

      IServiceMaturity serviceMaturity = new ServiceMaturity(_context);
      IServiceControlQueue serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, serviceMaturity);


      IServiceAccount serviceAccount = new ServiceAccount(_context, _contextLog, serviceControlQueue);
      IServicePerson servicePerson = new ServicePerson(_context, _contextLog, serviceControlQueue);
      IServiceIndicators serviceIndicators = new ServiceIndicators(_context, _contextLog, conn.TokenServer);
      IServiceParameters serviceParameters = new ServiceParameters(_context);
      IServiceAuthentication serviceAuthentication = new ServiceAuthentication(_context, _contextLog, serviceControlQueue);

      IServiceManager serviceManager = new ServiceManager(_contextStruct, serviceControlQueue, serviceBusConnectionString);


      serviceManager.UpdateStructManager();

      services.AddSingleton(_ => serviceControlQueue);
      services.AddSingleton(_ => serviceAccount);
      services.AddSingleton(_ => serviceAuthentication);
      services.AddSingleton(_ => servicePerson);
      services.AddSingleton(_ => serviceIndicators);
      services.AddSingleton(_ => serviceParameters);

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

      //services.AddSignalR();

      // Configurando o serviço de documentação do Swagger
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1",
            new Info
            {
              Title = "Indicators - Analisa Fluid Careers",
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
      app.UseCors("AllowAll");
      app.UseMvc();
      //app.UseSignalR(routes =>
      //{
      //  routes.MapHub<MessagesHub>("/messagesHub");
      //});
      // Ativando middlewares para uso do Swagger
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "help";
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "Indicators");
      });
    }
  }
}