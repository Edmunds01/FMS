using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using web_api.Middleware;
using web_api.Models;
using web_api.Repository;
using web_api.Services;
using web_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Convert Enums to Strings
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddDbContext<FMSContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "You are not authorized" });
            return context.Response.WriteAsync(result);
        }
    };
});

RegisterRepositoriesAndServices(builder.Services);

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

app.UseMiddleware<ConditionalAuthorizeMiddleware>();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

GenerateTypeScriptClientApi();

await app.RunAsync();


void RegisterRepositoriesAndServices(IServiceCollection services)
{
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<ITransactionRepository, TransactionRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();

    services.AddScoped<IAccountService, AccountService>();
    services.AddScoped<ITransactionService, TransactionService>();
    services.AddScoped<ICategoryService, CategoryService>();

    services.AddHttpContextAccessor();

    services.AddAutoMapper(typeof(Program));
}

void GenerateTypeScriptClientApi()
{
    // TODO: Prob add as a project reference
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    Task.Run(async () =>
    {
        await Task.Delay(1000);
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ../../api-client-generator https://localhost:5000/swagger/v1/swagger.json ../../client-app/src/api/auto-generated-client.ts",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        var error = await process.StandardError.ReadToEndAsync();
        if (!string.IsNullOrEmpty(error))
        {
            Debug.WriteLine("Error: " + error);
            Environment.Exit(1);
        }

        var output = await process.StandardOutput.ReadToEndAsync();
        Debug.WriteLineIf(!string.IsNullOrEmpty(output), "Message: " + output);

        await process.WaitForExitAsync();
    });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
}
