using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MemberAPI.Data.Database;
using MemberAPI.Data.Repository.v1;
using MemberAPI.Models.v1;
using MemberAPI.Data.Security.v1;
using MemberAPI.Security.v1;
using MemberAPI.Service.Plugins.v1;
using MemberAPI.Service.Master.v1;
using MemberAPI.Validators.v1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;

namespace MemberAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            NLog.LogManager.LoadConfiguration(System.String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddDbContextPool<MemberContext>(
                 options => options.UseSqlServer(Configuration.GetConnectionString("live")));
            services.AddAutoMapper(typeof(Startup));
            services.AddMvc()
                .AddFluentValidation()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressMapClientErrors = true;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Member API",
                    Description = "API to Register, Update and retrive member Information",
                    Contact = new OpenApiContact
                    {
                        Name = "Lijo Sebastian",
                        Email = "lijotech@gmail.com",
                        Url = new Uri("http://www.bluecomment.com/")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var actionExecutingContext =
                        actionContext as ActionExecutingContext;

                    if (actionContext.ModelState.ErrorCount > 0
                        && actionExecutingContext?.ActionArguments.Count == actionContext.ActionDescriptor.Parameters.Count)
                    {
                        return new UnprocessableEntityObjectResult(actionContext.ModelState);
                    }

                    return new BadRequestObjectResult(actionContext.ModelState);
                };
            });

            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitofWork, UnitofWork>();

            services.AddTransient<IValidator<CreateMemberModel>, CreateMemberModelValidator>();
            services.AddTransient<IValidator<UpdateMemberModel>, UpdateMemberModelValidator>();
            services.AddTransient<IValidator<ResetPasswordModel>, ResetPasswordModelValidator>();
            services.AddDataProtection();



            services.AddSingleton(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IServiceMaster, ServiceMaster>();

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddSingleton<DataProtectionPurposeStrings>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {           
            app.UseMiddleware<LoggingMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Member API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseRouting();
           
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
