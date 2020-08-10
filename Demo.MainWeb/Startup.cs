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
                options.LowercaseUrls = true; //��Դ·��Сд
            });
        }

        public ILifetimeScope AutofacContainer { get; private set; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();

            //����AOP���� 
            builder.RegisterDynamicProxy(config =>
            {
                //config.NonAspectPredicates.AddMethod("FindUser");
                //config.Interceptors.AddTyped<LogInterceptorAttribute>();

                //config.NonAspectPredicates.AddNamespace("*.Business");
            });

            //builder.RegisterDynamicProxy(config =>
            //{
            //    //ȫ��ʹ��AOP  �������ڲ���ʹ�õĽӿڵķ�ʽ����Ҫ��Ҫʹ��AOP�ķ����ϼ� virtual �ؼ���
            //    config.Interceptors.AddTyped<LogInterceptorAttribute>();
            //    config.Interceptors.AddServiced<LogInterceptorAttribute>();
            //    // ����Service��׺��ǰ�����ᱻ����
            //    config.Interceptors.AddTyped<LogInterceptorAttribute>(method => method.Name.EndsWith("Service"));
            //    // ʹ�� ͨ��� ���ض�ȫ��������
            //    config.Interceptors.AddTyped<LogInterceptorAttribute>(Predicates.ForService("*Service"));

            //    //Demo.Data�����ռ��µ�Service���ᱻ����
            //    config.NonAspectPredicates.AddNamespace("Demo.Data");

            //    //���һ��ΪData�������ռ��µ�Service���ᱻ����
            //    config.NonAspectPredicates.AddNamespace("*.Data");

            //    //ICustomService�ӿڲ��ᱻ����
            //    config.NonAspectPredicates.AddService("ICustomService");

            //    //��׺ΪService�Ľӿں��಻�ᱻ����
            //    config.NonAspectPredicates.AddService("*Service");

            //    //����ΪFindUser�ķ������ᱻ����
            //    config.NonAspectPredicates.AddMethod("FindUser");

            //    //��׺ΪUser�ķ������ᱻ����
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
