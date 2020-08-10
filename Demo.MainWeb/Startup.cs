using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Demo.Business;
using Demo.IocCommon;
using Demo.IocCommon.AopCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo.MainWeb
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;

            });
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddControllers()
                .AddControllersAsServices();

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true; //资源路径小写
            });
        }

        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();

            //配置AOP代理 
            builder.RegisterDynamicProxy(config =>
            {
                //config.NonAspectPredicates.AddMethod("FindUser");
                //config.Interceptors.AddTyped<LogInterceptorAttribute>();

                //config.NonAspectPredicates.AddNamespace("*.Business");
            });

            //builder.RegisterDynamicProxy(config =>
            //{
            //    //全局使用AOP  这里由于不是使用的接口的方式，需要在要使用AOP的方法上加 virtual 关键字
            //    config.Interceptors.AddTyped<LogInterceptorAttribute>();
            //    config.Interceptors.AddServiced<LogInterceptorAttribute>();
            //    // 带有Service后缀当前方法会被拦截
            //    config.Interceptors.AddTyped<LogInterceptorAttribute>(method => method.Name.EndsWith("Service"));
            //    // 使用 通配符 的特定全局拦截器
            //    config.Interceptors.AddTyped<LogInterceptorAttribute>(Predicates.ForService("*Service"));

            //    //Demo.Data命名空间下的Service不会被代理
            //    config.NonAspectPredicates.AddNamespace("Demo.Data");

            //    //最后一级为Data的命名空间下的Service不会被代理
            //    config.NonAspectPredicates.AddNamespace("*.Data");

            //    //ICustomService接口不会被代理
            //    config.NonAspectPredicates.AddService("ICustomService");

            //    //后缀为Service的接口和类不会被代理
            //    config.NonAspectPredicates.AddService("*Service");

            //    //命名为FindUser的方法不会被代理
            //    config.NonAspectPredicates.AddMethod("FindUser");

            //    //后缀为User的方法不会被代理
            //    config.NonAspectPredicates.AddMethod("*User");
            //});
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            this.AutofacContainer = app.ApplicationServices.CreateScope().ServiceProvider.GetAutofacRoot();

            IocCore.AutofacContainer = AutofacContainer;

            var a = AutofacContainer?.IsRegistered<UserBusiness>();
            var a1 = AutofacContainer?.Resolve<UserBusiness>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapGet("/", async context =>
               {
                   await context.Response.WriteAsync("Hello World!");
               });
            });
        }
    }
}
