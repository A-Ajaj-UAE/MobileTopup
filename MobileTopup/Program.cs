
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MobileTopup.API.Middleware;
using MobileTopup.API.Repositories;
using MobileTopup.API.Services;
using MobileTopup.API.Settings;
using MobileTopup.Contracts.Domain.Entities;
using MobileTopup.Contracts.MappingProfile;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Validatiors;
using MobileTopup.Infrastructure;
using System.Data;

namespace MobileTopup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // ignore json reference loop in add controllers
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApiVersioning(options =>
            {
                //indicating whether a default version is assumed when a client does
                // does not provide an API version.
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            //add application db support
            string connection = builder.Configuration.GetConnectionString("DBConnection");
            builder.Services.AddTransient<IDbConnection>((sp) => new SqlConnection(connection));

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(connection,
                sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("MobileTopup.API");
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        sqlOptions.CommandTimeout(180); // Set the command timeout to 180 seconds
                        ;
                    });
            });

            // Register validators and services
            builder.Services.AddScoped<IValidator<User>, UserValidator>();
            builder.Services.AddScoped<IValidator<AddBeneficiaryRequest>, BeneficiaryValidator>();
            builder.Services.AddScoped<IMobileTopupService, MobileTopupService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAccountService, AccountService>();

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
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            


            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlerMiddleware>();


            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                db.Database.Migrate();
            }


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
