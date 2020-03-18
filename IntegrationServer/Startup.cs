using System;
using System.IO;
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
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Tools;

namespace IntegrationServer
{
    /// <summary>
    /// Controlador de Inicialização do Projeto
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="configuration">Configurador</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Propriedade publica do configurador
        /// </summary>
        public IConfiguration Configuration { get; }
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";
        /// <summary>
        /// Configurador de serviços
        /// </summary>
        /// <param name="services">Coleção de serviços</param>
        public void RegistreServices(IServiceCollection services)
        {
            DataContext _context;

            var conn = XmlConnection.ReadVariablesSystem();
            _context = new DataContext(conn.Server, conn.DataBase);

            DataContext _contextLog;
            _contextLog = new DataContext(conn.ServerLog, conn.DataBaseLog);

            DataContext _contextIntegration;
            _contextIntegration = new DataContext(conn.ServerIntegration, conn.DataBaseIntegration);

            var serviceBusConnectionString = conn.ServiceBusConnectionString;
            IServiceMaturity serviceMaturity = new ServiceMaturity(_context);
            IServiceControlQueue serviceControlQueue = new ServiceControlQueue(serviceBusConnectionString, serviceMaturity);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            IServiceAccount serviceAccount = new ServiceAccount(_context, _contextLog, serviceControlQueue);
            IServiceAudit serviceAudit = new ServiceAudit(_context);
            IServiceCompany serviceCompany = new ServiceCompany(_context);
            IServiceInfra serviceInfra = new ServiceInfra(_context);
            IServiceIntegration serviceIntegration = new ServiceIntegration(_context, _contextLog, _contextIntegration, serviceControlQueue, conn.SignalRService);
            IServiceLog serviceLog = new ServiceLog(_context, _contextLog);
            IServiceParameters serviceParameters = new ServiceParameters(_context);
            IServicePerson servicePerson = new ServicePerson(_context, _contextLog, serviceControlQueue, conn.SignalRService);
            IServiceUser serviceUser = new ServiceUser(_context, _contextLog);
            IServiceWorkflow serviceWorkflow = new ServiceWorkflow(_context, _contextLog, serviceControlQueue, conn.SignalRService);
            IServiceAuthentication serviceAuthentication = new ServiceAuthentication(_context, _contextLog, serviceControlQueue, conn.SignalRService);

            services.AddSingleton(_ => serviceAuthentication);
            services.AddSingleton(_ => serviceControlQueue);
            services.AddSingleton(_ => serviceAccount);
            services.AddSingleton(_ => serviceAudit);
            services.AddSingleton(_ => serviceCompany);
            services.AddSingleton(_ => serviceInfra);
            services.AddSingleton(_ => serviceIntegration);
            services.AddSingleton(_ => serviceLog);
            services.AddSingleton(_ => serviceParameters);
            services.AddSingleton(_ => servicePerson);
            services.AddSingleton(_ => serviceUser);
            services.AddSingleton(_ => serviceWorkflow);
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

            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
      {
          c.SwaggerDoc("v1",
              new Microsoft.OpenApi.Models.OpenApiInfo
              {
                  Title = "Integration Server - Integração de Colaboradores",
                  Version = "v1",
                  Description = "Responsável pelas ferramentas de integração de colaboradores do Fluid"
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
        /// Configurador de ambiente
        /// </summary>
        /// <param name="app">Aplicação</param>
        /// <param name="env">Ambiente de hospedagem</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "help";
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Integration Server");
            });
        }
    }
}
