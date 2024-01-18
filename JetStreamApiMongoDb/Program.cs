
using JetStreamApiMongoDb.Common;
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
using System.Linq;
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
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

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
            var userService = scopedServices.GetRequiredService<IUserService>();

            if (!await dbContext.Users.Any() && !await dbContext.Priorities.Any() && !await dbContext.Services.Any() && !await dbContext.Statuses.Any() && !await dbContext.OrderSubmissions.Any())
            {

                await userService.CreateUserAsync("admin", "Password", Roles.ADMIN);
                await userService.CreateUserAsync("admin1", "Password1", Roles.ADMIN);
                await userService.CreateUserAsync("user", "Password", Roles.USER);

                for (int i = 1; i <= 10; i++)
                {
                    await userService.CreateUserAsync($"user{i}", $"Password{i}", Roles.USER);
                }

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

                var firstnames = new List<string>
                {
                    "Max", "Anna", "John", "Jane", "Peter", "Paula", "Tom", "Tina", "Robert", "Rebecca",
                    "Martin", "Emily", "Daniel", "Sophia", "David", "Olivia", "Michael", "Isabella", "James", "Mia"
                };
                var lastnames = new List<string>
                {
                    "Mustermann", "Musterfrau", "Doe", "Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis",
                    "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore", "Martin"
                };

                var emails = new List<string>
                {
                    "max.mustermann@example.com", "anna.musterfrau@example.com", "john.doe@example.com", "jane.doe@example.com",
                    "peter.parker@example.com", "paula.paulsen@example.com", "tom.thompson@example.com", "tina.turner@example.com",
                    "robert.robertson@example.com", "rebecca.richards@example.com", "martin.martinson@example.com", "emily.elliot@example.com",
                    "daniel.davison@example.com", "sophia.simpson@example.com", "david.dickinson@example.com", "olivia.olson@example.com",
                    "michael.michaels@example.com", "isabella.isaacson@example.com", "james.jacobson@example.com", "mia.morrison@example.com"
                };

                var phones = new List<string>
                {
                    "012 345 67 89", "023 456 78 90", "034 567 89 01", "045 678 90 12", "056 789 01 23", "067 890 12 34", "078 901 23 45",
                    "089 012 34 56", "090 123 45 67", "101 234 56 78", "112 345 67 89", "123 456 78 90", "134 567 89 01", "145 678 90 12",
                    "156 789 01 23", "167 890 12 34", "178 901 23 45", "189 012 34 56", "190 123 45 67", "201 234 56 78"
                };

                var comments = new List<string>
                {
                    "Bitte die Skier gut wachsen, ich fahre am Wochenende in die Berge.", "Ich brauche die Skier bis Freitag.",
                    "Bitte die Bindung auf meine neue Schuhgröße einstellen.", "Die Kanten meiner Skier sind sehr stumpf, bitte schärfen.",
                    "Meine Skier haben einen tiefen Kratzer, kann das repariert werden?", "Ich brauche einen kompletten Service für meine Skier.",
                    "Bitte die Skier für Slalomrennen vorbereiten.", "Ich brauche neue Felle für meine Tourenskier.",
                    "Bitte die Skier für Pulverschnee präparieren.", "Ich brauche einen Service für meine Snowboard.",
                    "Bitte die Bindung meiner Snowboard überprüfen.", "Können Sie meine Skischuhe anpassen?",
                    "Ich brauche neue Skistöcke, können Sie mir welche empfehlen?", "Bitte meine Skier für das Rennen am Samstag vorbereiten.",
                    "Können Sie meine Skier für das Springen präparieren?", "Ich brauche einen Service für meine Langlaufskier.",
                    "Bitte meine Skier für das Freeriden vorbereiten.", "Können Sie meine Skier für das Carven präparieren?",
                    "Ich brauche einen Service für meine Telemark-Skier.", "Bitte meine Skier für das Gelände vorbereiten."
                };

                var orderSubmissions = new List<OrderSubmission>();

                Random rnd = new Random();

                for (int i = 0; i < 20; i++)
                {
                    var randomPriorityId = priorities[rnd.Next(priorities.Count)];
                    var randomServiceId = services[rnd.Next(services.Count)];

                    var selectedPriotity = await dbContext.Priorities.FindWithProxies(Builders<Priority>.Filter.Eq(p => p.Id, randomPriorityId));
                    var selectedService = await dbContext.Services.FindWithProxies(Builders<Service>.Filter.Eq(s => s.Id, randomServiceId));

                    var totalPrice = (selectedPriotity.FirstOrDefault()?.Price ?? 0) + (selectedService.FirstOrDefault()?.Price ?? 0);

                    orderSubmissions.Add(new OrderSubmission
                    {
                        Firstname = firstnames[i],
                        Lastname = lastnames[i],
                        Email = emails[i],
                        Phone = phones[i],
                        PriorityId = randomPriorityId,
                        CreateDate = DateTime.Now.AddDays(rnd.Next(1, 5)),
                        PickupDate = DateTime.Now.AddDays(rnd.Next(1, 20)),
                        ServiceId = randomServiceId,
                        StatusId = offenStatusId,
                        TotalPrice_CHF = totalPrice,
                        Comment = comments[i]
                    });
                }
                await dbContext.OrderSubmissions.SeedDatabase(orderSubmissions);
            }
        }
    }
}

