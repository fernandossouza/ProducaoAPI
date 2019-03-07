using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProducaoAPI.Service;
using ProducaoAPI.Service.Interface;
using ProducaoAPI.Models.Repository;

namespace ProducaoAPI
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
            services.AddCors (o => o.AddPolicy ("CorsPolicy", builder => {
                builder.AllowAnyOrigin ()
                    .AllowAnyMethod ()
                    .AllowAnyHeader ();
            }));
            services.AddMvc();
            
            // Service
            services.AddTransient<IProgramacaoService,ProgramacaoService>();
            services.AddTransient<IIntegracaoAPIService,IntegracaoAPIService>();

            //Repository
            services.AddTransient<TbProgramacaoRepository,TbProgramacaoRepository>();
            services.AddTransient<TbProgramacaoAlocacaoRepository,TbProgramacaoAlocacaoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors ("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
