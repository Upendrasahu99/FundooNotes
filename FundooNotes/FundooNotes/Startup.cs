using BusinessLayer.Interface;
using BusinessLayer.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepoLayer.Context;
using RepoLayer.Interface;
using RepoLayer.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet;
using MassTransit;

namespace FundooNotes
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
            // Database Configuration
            services.AddDbContext<FundooContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:FundooDB"]));

            //RabbitMQ Configuration for Producer

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();
            
            //User Configuration
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IUserRepo, UserRepo>();

            //Note Configuration
            services.AddTransient<INoteBusiness, NoteBusiness>();
            services.AddTransient<INoteRepo,  NoteRepo>();
            
            //Collab Configuration
            services.AddTransient<ICollabBusiness, CollabBusiness>();
            services.AddTransient<ICollabRepo, CollabRepo>(); 

            //Label Configuration
            services.AddTransient<ILabelBusiness, LabelBusiness>();
            services.AddTransient<ILabelRepo, LabelRepo>();

            //Add Swagger Generator           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Using the Authorization header with Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };
                c.AddSecurityDefinition(securitySchema.Reference.Id,securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { {
                    securitySchema, Array.Empty<string>()}});
            });

            var Token = Configuration.GetValue<string>("JwtSettings:SecretKey");

           
            // Configuration of JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Token))
                };
            });

            //Cloudinary Configuration
            IConfiguration configurationSection = Configuration.GetSection("Cloudinary");
            Account cloudinaryAccount = new Account(
                configurationSection["CloudName"],
                configurationSection["ApiKey"],
                configurationSection["ApiSecret"]
                );
            Cloudinary cloudinary = new Cloudinary( cloudinaryAccount );
            services.AddSingleton(cloudinary); 

            services.AddTransient<FileService,  FileService>();


            // Radius configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
            });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger  middleware Implementaion
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/Swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            // Aitjemtocatopm is above authorization
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
            
        }
    }
}
