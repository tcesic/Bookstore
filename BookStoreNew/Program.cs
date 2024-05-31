using BookStore;
using BookStore.Repositories;
using DBContext.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Konfiguracija IdentityServer4
builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityConfig.GetClients())
    .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
    .AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
    .AddInMemoryApiResources(IdentityConfig.GetApiResources())
    .AddDeveloperSigningCredential();

// Konfiguracija JWT autentikacije
var key = Encoding.ASCII.GetBytes(IdentityConfig.SecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = IdentityConfig.Url; // URL of IdentityServer
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = IdentityConfig.Url,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ImplicitPolicy", policy =>
    {
        policy.RequireClaim("scope", "api1");
    });

    options.AddPolicy("ClientCredentialsPolicy", policy =>
    {
        policy.RequireClaim("scope", "api2");
    });
});


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<BookstoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bookstore API", Version = "v1" });

    // Configure OAuth2
    var securityScheme = new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(IdentityConfig.AutorizationUrl),
                Scopes = new Dictionary<string, string>
                    {
                        { "api1", "Implicit API" }
                    }
            },
            ClientCredentials = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri(IdentityConfig.TokenConnectUrl),
                Scopes = new Dictionary<string, string>
                    {
                        { "api2", "Client credentials API" }
                    }
            }
        },
    };

    // Bearer Token Security Definition
    var securitySchemeBearer = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "oauth2"
    };


    c.AddSecurityDefinition(SecuritySchemeType.OAuth2.ToString(), securityScheme);

    c.AddSecurityDefinition("Bearer", securitySchemeBearer);

    var securityRequirement = new OpenApiSecurityRequirement
        {
            { securityScheme, new[] { "api1","api2" } }, 
        };

    c.AddSecurityRequirement(securityRequirement);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new[] { "api1","api2" }
            }
        });

});

var app = builder.Build();

// Seed the database
SeedDatabase(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookstore API V1");

        // Enable OAuth2
        
        c.OAuthClientId(IdentityConfig.ClientCredentialsClient);
        c.OAuthClientSecret(IdentityConfig.SecretKey);
        c.OAuthAppName("My API - Swagger");
        c.OAuthUsePkce(); // Use PKCE if you're using it
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
     endpoints.MapGet("/", context => {
         context.Response.Redirect("/swagger/index.html");
         return Task.CompletedTask;
     });
    endpoints.MapControllers();
}); 

app.MapControllers();
app.Run();

void SeedDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<BookstoreContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}