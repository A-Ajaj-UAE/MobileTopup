
using FluentValidation;
using MobileTopup.API.Repositories;
using MobileTopup.API.Services;
using MobileTopup.API.Settings;
using MobileTopup.Contracts.Models;
using MobileTopup.Contracts.Validatiors;

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

            // register settings
            builder.Services.Configure<BalanceEndpoint>(builder.Configuration.GetSection("BalanceEndpoint"));
            builder.Services.Configure<TopupSettings>(builder.Configuration.GetSection("TopupSettings"));


            //register http client
            builder.Services.AddHttpClient<IUserService,UserService>((serviceProvider, client) =>
            {
                var baseAddress = builder.Configuration.GetSection("BalanceEndpoint").GetValue<string>("BaseAdress");
                client.BaseAddress = new Uri(baseAddress);
            });

            //register repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITopupRepository, TopupRepository>();

       

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
