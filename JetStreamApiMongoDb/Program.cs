
using JetStreamApiMongoDb.Data;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using JetStreamApiMongoDb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using System.Text;

namespace JetStreamApiMongoDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Serilogger
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            // Add Automapper
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IMongoDbContext, MongoDbContext>();

            // Add services to the container.
            builder.Services.AddScoped<IOrderSubmissionService, OrderSubmissionService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JestreamApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //Add JWT Authentification 
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            SeedDatabase(app.Services).Wait();

            app.Run();
        }

        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<IMongoDbContext>();

            if (!await dbContext.Priorities.Any() && !await dbContext.Services.Any() && !await dbContext.Statuses.Any() && !await dbContext.OrderSubmissions.Any())
            {
                var priorities = await dbContext.Priorities.SeedDatabase(new List<Priority>
                {
                    new () { Name = "Tief", Price = 0 },
                    new () { Name = "Standard", Price = 5 },
                    new () { Name = "Hoch", Price = 10 }
                });

                var services = await dbContext.Services.SeedDatabase(new List<Service>
                {
                    new () { Name = "Kleiner Service", Price = 49 },
                    new () { Name = "Grosser Service", Price = 69 },
                    new () { Name = "Rennskiservice", Price = 99 },
                    new () { Name = "Bindung montieren und einstellen", Price = 39 },
                    new () { Name = "Fell zuschneiden", Price = 25 },
                    new () { Name = "Heisswachsen", Price = 18 }
                });

                var statuses = await dbContext.Statuses.SeedDatabase(new List<Status>
                {
                    new () { Name = "Offen" },
                    new () { Name = "In Arbeit" },
                    new () { Name = "Abgeschlossen" },
                    new () { Name = "Storniert" }
                });

                var offenStatusList = await dbContext.Statuses.FindWithProxies(Builders<Status>.Filter.Eq(s => s.Name, "Offen"));
                var offenStatus = offenStatusList.FirstOrDefault();
                if (offenStatus == null)
                {
                    throw new InvalidOperationException("Der Status 'Offen' konnte nicht gefunden werden.");
                }
                var offenStatusId = offenStatus.Id;

                Random rnd = new Random();

                var randomPriorityId = priorities[rnd.Next(priorities.Count)];
                var randomServiceId = services[rnd.Next(services.Count)];

                var selectedPriotity = await dbContext.Priorities.FindWithProxies(Builders<Priority>.Filter.Eq(p => p.Id, randomPriorityId));
                var selectedService = await dbContext.Services.FindWithProxies(Builders<Service>.Filter.Eq(s => s.Id, randomServiceId));

                var totalPrice = (selectedPriotity.FirstOrDefault()?.Price ?? 0) + (selectedService.FirstOrDefault()?.Price ?? 0);

                var orderSubmissions = await dbContext.OrderSubmissions.SeedDatabase(new List<OrderSubmission>
                {
                    new ()
                    {
                        Firstname = "Max",
                        Lastname = "Mustermann",
                        Email = "max.mustermann@example.com",
                        Phone = "012 345 67 89",
                        PriorityId = randomPriorityId,
                        CreateDate = DateTime.Now,
                        PickupDate = DateTime.Now.AddDays(7),
                        ServiceId = randomServiceId,
                        StatusId = offenStatusId,
                        TotalPrice_CHF = totalPrice,
                        Comment = "Bitte die Skier gut wachsen, ich fahre am Wochenende in die Berge."
                    }
                });
            }
        }
    }
}
