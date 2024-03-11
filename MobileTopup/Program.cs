
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MobileTopup.API.Repositories;
using MobileTopup.API.Services;
using MobileTopup.API.Settings;
using MobileTopup.Contracts.Models;
using MobileTopup.Contracts.Validatiors;
using System.Configuration;

namespace MobileTopup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApiVersioning(options =>
            {
                //indicating whether a default version is assumed when a client does
                // does not provide an API version.
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            // Register validators and services
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<Beneficiary>, BeneficiaryValidator>();
            builder.Services.AddScoped<IMobileTopupService, MobileTopupService>();
            builder.Services.AddScoped<IUserService, UserService>();

            //register http client
            builder.Services.AddHttpClient<UserService>();

            //register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITopupRepository, TopupRepository>();

            // register settings
            builder.Services.Configure<TopupSettings>(builder.Configuration.GetSection("TopupSettings"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
