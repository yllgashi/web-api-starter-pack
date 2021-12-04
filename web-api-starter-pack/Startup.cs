using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.OpenApi.Models;


namespace web_api_starter_pack
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
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());


            //    options.AddPolicy("SameOrigin",
            //        builder => builder.WithOrigins(
            //            "http://localhost:4200"
            //        ).AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            //});

            Models.DbHelper.ConnectionString = Configuration["ConnectionStrings:ConnectionString"];

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "web_api_starter_pack", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "web_api_starter_pack v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseCors("SameOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
