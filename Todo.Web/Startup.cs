using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Data.Models;
using Todo.Business.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Todo.Data.Context;
using Todo.Business.Services.Database;
using AutoMapper;
using Todo.ApiClient;

namespace Todo
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
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddMaps(new[] {
                                "Todo.Web",
                                "Todo.Business"
                })
            );
            IMapper mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();

            services.AddTransient<IDataProviderAsync<TodoItemVo>, InDbTodoItemProvider>();
            services.AddTransient<IDataProviderAsync<CategoryVo>, InDbCategoryProvider>();
            services.AddTransient<IDataProviderAsync<TagVo>, InDbTagProvider>();
            services.AddTransient<ITodoItemTagProviderAsync, InDbTodoItemTagProvider>();
            services.AddSingleton(new ClientsClass("https://localhost:44384"));

            services.AddDbContext<Data.Context.AppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AppContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
