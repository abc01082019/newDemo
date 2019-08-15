using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogDemo.Api.Extensions;
using BlogDemo.Core.Interface;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Repository;
using BlogDemo.Infrastructure.Resources;
using BlogDemo.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace BlogDemo.Api
{
    public class StartupDevelopment
    {
        public static IConfiguration Configuration { get; set; }

        public StartupDevelopment(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Register Mvc
            services.AddMvc(options => 
            {
                options.ReturnHttpNotAcceptable = true;
                // Accept xml format
                //options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                // If Json format is allowed then we define our own media type(s)
                var inputFormatter = options.InputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
                if (inputFormatter != null)
                {
                    inputFormatter.SupportedMediaTypes.Add("application/vnd.cgzl.post.create+json");
                    inputFormatter.SupportedMediaTypes.Add("application/vnd.cgzl.post.update+json");
                }
                var outputFormatter = options.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();
                if (outputFormatter != null)
                {
                    outputFormatter.SupportedMediaTypes.Add("application/vnd.cgzl.hateoas+json");
                }

            })
            .AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddFluentValidation();

            // 注册Https, 会强制使用Https来浏览: 端口为5001
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });

            // 将自定义的MyContext注入到容器中
            services.AddDbContext<MyContext>(options =>
            {
                //var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlite(connectionString);
            });

            // 接口和实体类对接
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(Startup));

            // 验证类和实体类对接
            services.AddTransient<IValidator<PostAddResource>, PostAddOrUpdateResourceValidator<PostAddResource>>();
            services.AddTransient<IValidator<PostUpdateResource>, PostAddOrUpdateResourceValidator<PostUpdateResource>>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            // 排序
            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<PostPropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            services.AddTransient<ITypeHelperService, TypeHelperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            //app.UseDeveloperExceptionPage();    // 错误是返回一个页面, 对Mvc开发者有用
            app.UseMyExceptionHandler(loggerFactory);

            app.UseHttpsRedirection();  // 在Mvc之前启动强制使用Https服务

            app.UseMvc();
        }
    }
}
