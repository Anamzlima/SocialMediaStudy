using Application.Activities;
using Persistence;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Application.Core;
using API.Extensions;
using FluentValidation.AspNetCore;
using API.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // aqui é o dependency injection container onde injetamos quaisquer serviços que quisermos consumir
        // na aplicação
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt => 
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddFluentValidation(config => 
            {
                config.RegisterValidatorsFromAssemblyContaining<Create>();
            });
            services.AddApplicationServices(_config);
            services.AddIdentityServices(_config);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // aqui pode adicionar middleware, que pode fazer alguma coisa com o request que entra e sai da pipeline
        // a ordem do que acontece aqui é importante
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            //qualquer recurso que chega como HTTP vai ser redirecionado para HTTPS
            // app.UseHttpsRedirection();

            //middleware para permitir o roteamento para o controller certo
            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            //mapeia os endpoints do controller na API, para que o servidor da API saiba o que fazer 
            //quando chegar um request para a API e como rotear para o devido controller
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
