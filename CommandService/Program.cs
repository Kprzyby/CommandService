using CommandService.Data;
using CommandService.Data.Repos;
using CommandService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;

    if (env.IsProduction())
    {
        builder.Services.AddDbContext<DataContext>(options =>
        {
            string connectionString = builder.Configuration.GetConnectionString("K8SDBConnection");

            options.UseSqlServer(connectionString);
        });
    }
    else
    {
        builder.Services.AddDbContext<DataContext>(options =>
        {
            string connectionString = builder.Configuration.GetConnectionString("DBConnection");

            options.UseSqlServer(connectionString);
        });
    }

    config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //load base settings
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true) //load local settings
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true) //load environment settings
                .AddEnvironmentVariables();
});

builder.Services.AddScoped<CommandRepo>();
builder.Services.AddScoped<CommandServ>();

builder.Services.AddScoped<PlatformRepo>();
builder.Services.AddScoped<PlatformService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//if in production environment, apply migrations
if (app.Environment.IsProduction())
{
    DBPrep.ApplyMigrations(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();