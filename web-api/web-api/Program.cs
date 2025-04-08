using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json.Serialization;
using web_api.Helper;
using web_api.Helper.Interfaces;
using web_api.Middleware;
using web_api.Models;
using web_api.Repository;
using web_api.Repository.Interfaces;
using web_api.Services;
using web_api.Services.Interfaces;

const string CorsPolicyName = "SpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var foo = "Server=localhost\\SQLEXPRESS;Database=FMS;Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<FMSContext>(options => options.UseSqlServer(foo)
        .EnableSensitiveDataLogging());

builder.Services.AddCors(options => options.AddPolicy(CorsPolicyName, policy =>
    {
        var allowedOrigin = builder.Configuration["ALLOWED_ORIGIN"];

        policy.AllowAnyMethod()
              .AllowCredentials()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin =>
              {
                  var host = new Uri(origin).Host;

                  return host == "localhost" || host == allowedOrigin;
              });
    }));

RegisterRepositoriesAndServices(builder.Services);

builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FMSContext>();
    await dbContext.Database.MigrateAsync();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

app.UseCors(CorsPolicyName);

app.UseMiddleware<ConditionalAuthorizeMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

// TODO: check if this will work on deploy
//app.UseHttpsRedirection();

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

    services.AddSingleton<ITokenHelper, TokenHelper>();

    services.AddHttpContextAccessor();

    services.AddAutoMapper(typeof(Program));
}

void GenerateTypeScriptClientApi()
{
    // TODO: Prob add as a project reference
    Task.Run(async () =>
    {
        await Task.Delay(1000);
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --project ../../api-client-generator http://localhost:5000/swagger/v1/swagger.json ../../client-app/src/api/auto-generated-client.ts",
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
        }

        var output = await process.StandardOutput.ReadToEndAsync();
        Debug.WriteLineIf(!string.IsNullOrEmpty(output), "Message: " + output);

        await process.WaitForExitAsync();
    });
}
