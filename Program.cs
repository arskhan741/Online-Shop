using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Online_Shop.Configurations;
using Online_Shop.Contracts;
using Online_Shop.Models;
using Online_Shop.Repository;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //For DB connection
        //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

        // For PG connection
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PGConn")));



        // For Identity  
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();

        // Adding Authentication  
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Adding Jwt Bearer
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
            };
        });

        //Adding policy to authorization
        builder.Services.AddAuthorization(options =>
        {
            //Hiearchical role based Policy
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Admin", "Manager"));
            options.AddPolicy("MemberPolicy", policy => policy.RequireRole("Admin", "Manager", "Member"));
            options.AddPolicy("UserPolicy", policy => policy.RequireRole("Admin", "Manager", "Member", "User"));

            //Over 21 Age Policy
            options.AddPolicy("Over21", policy => policy.RequireAssertion(context =>
                context.User.HasClaim(c =>
                c.Type == ClaimTypes.DateOfBirth && DateTime.Parse(c.Value) <= DateTime.Today.AddYears(-21))));

            // Adding Azhar policy
            options.AddPolicy("Claims_Azhar", policy =>
                policy.RequireClaim("Claim_Azhar", "Value_Azhar"));

            // Adding role claim policy
            options.AddPolicy("Claims_Admin", policy =>
                policy.RequireClaim("Claim_Admin", "Value_Admin"));

            // Combined policy
            options.AddPolicy("CombinedPolicy", policy =>
            {
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Claim_Azhar" && c.Value == "Value_Azhar") &&
                    context.User.HasClaim(c => c.Type == "Claim_Admin" && c.Value == "Value_Admin"));
            });

        });

        // Add services to the container.
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IRoleService, RoleRepository>();
        builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
        builder.Services.AddTransient<IProductRepository, ProductRepository>();

        builder.Services.AddAutoMapper(typeof(MapperConfig));

        // Add MemoryCache service
        builder.Services.AddMemoryCache();

        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });

        // Register for swagger Controller
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Add Security Definition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space of your token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            // Add Security Requirement
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
                    Array.Empty<string>()
                }
            });

            // Add this line to support enums as strings in Swagger
            c.UseAllOfToExtendReferenceSchemas();
        });

        // Enable support for Newtonsoft.Json in Swashbuckle
        builder.Services.AddSwaggerGenNewtonsoftSupport();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "Manager", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Configure the HTTP request pipeline.
        // Middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
        }

        app.UseHttpsRedirection();

        app.UseAuthentication(); // it is new line

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
