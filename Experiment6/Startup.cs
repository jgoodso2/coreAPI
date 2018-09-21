using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
//using NLog.Extensions.Logging;
using ProjectInfo.API.Services;
using Microsoft.Extensions.Configuration;
using ProjectInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ProjectInfo.API
{
    public class Startup
    {
       
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc(
                options =>
                {
#if DEBUG
                    options.SslPort = 44321;
#else
                    options.SslPort = 443;
#endif
                    options.Filters.Add(new RequireHttpsAttribute());
                }

                )
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter()));
            //.AddJsonOptions(o => {
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver
            //            as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            //});

            //#if DEBUG
            //            ///services.AddTransient<IMailService, LocalMailService>();
            //#else
            //            services.AddTransient<IMailService, CloudMailService>();
            //#endif
            //var connectionString = Configuration["connectionStrings:ProjectInfoDBConnectionString"];
            var connectionString = "Server= MARTIN;Database=mapperDB;Integrated Security=SSPI;";
            services.AddDbContext<ProjectContext>(o => o.UseSqlServer(connectionString));

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;

#if DEBUG
                options.HttpsPort = 443;
#else
                options.HttpsPort = 443;
#endif
            });
            services.AddScoped<IProjectInfoRepository, ProjectInfoRepository>();

            services.AddAntiforgery(
        options =>
        {
            options.Cookie.Name = "_prjinfo";
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.HeaderName = "X-XSRF-TOKEN";
        }
    );
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //        builder =>
            //        {
            //            builder
            //            .AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials();
            //        });
            //});
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //var connectionString = Configuration["connectionStrings:ProjectInfoDBConnectionString"];
            ////var connectionString = Startup.Configuration["connectionStrings:ProjectInfoDBConnectionString"];
            //services.AddDbContext<ProjectContext>(o => o.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseMvc();
            app.UseCors("AllowAll");

            //loggerFactory.AddNLog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseExceptionHandler();
            }

            //ProjectInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();
            app.UseHttpsRedirection(); // to enfore https redirection


            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Project, Models.ProjectWithoutPlanViewProjectsDto>();
                cfg.CreateMap<Entities.Project, Models.ProjectDto>();
                cfg.CreateMap<Entities.PlanViewProject, Models.PlanViewProjectDto>();
                cfg.CreateMap<Models.PlanViewProjectsForCreationDto, Entities.PlanViewProject>();
                cfg.CreateMap<Models.PlanViewProjectsForUpdateDto, Entities.PlanViewProject>();
                cfg.CreateMap<Entities.PlanViewProject, Models.PlanViewProjectsForUpdateDto>();
                cfg.CreateMap<Entities.AuthorizedPlanViewProject, Models.AuthorizedPlanViewProjectDto>();
                cfg.CreateMap<Models.AuthorizedPlanViewProjectDto, Entities.AuthorizedPlanViewProject>();
                cfg.CreateMap<Models.ProjectDto, Entities.Project>();
                cfg.CreateMap<Models.ProjectDto, Entities.Project>()
                    .ForMember(dest => dest.PlanViewProjects, opt => opt.MapFrom(src => src.PlanViewProjects));

                cfg.CreateMap<Entities.Project, Models.ProjectDto>()
                   .ForMember(dest => dest.PlanViewProjects, opt => opt.MapFrom(src => src.PlanViewProjects));

            });

            app.UseMvc();

        }
    }
}
