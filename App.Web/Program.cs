using System.Reflection;
using App.Core;
using App.Entity;
using App.Web.Fx;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

dotenv.net.DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);
builder.WebHost
    .ConfigureAppConfiguration(p =>
    {
        p.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        p.AddEnvironmentVariables();
    })
    .ConfigureKestrel(p=>p.AllowSynchronousIO = true);
// Add services to the container.
WebHelper appConfig = builder.Configuration.GetSection(WebHelper.APP_SETTINGS_SECTION).Get<WebHelper>();
builder.Services.AddSingleton(appConfig);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
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
            new string[] { }
        }
    });
});
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<WebHelper.SessionHelper>();
builder.Services.AddScoped<NeoContext>();
builder.Services.AddScoped<NeoAuthorization>();
builder.Services.Configure<WebHelper>(builder.Configuration.GetSection(WebHelper.APP_SETTINGS_SECTION));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o=>NeoAuthorization.JwtOptions(o, builder.Configuration));
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(_ =>
{
    var connectionString = appConfig.AppConnectionString;
    _.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),b=>b.MigrationsAssembly(typeof(Program).Assembly.FullName));
    // _.UseNpgsql(connectionString,b=>b.MigrationsAssembly(typeof(Program).Assembly.FullName));
});
builder.Services.AddCors();

LoadServiceRegistrars(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(p =>
{
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.AllowAnyOrigin();
});
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void LoadServiceRegistrars(WebApplicationBuilder webApplicationBuilder)
{
    // Get all loaded assemblies
    var assemblies = Directory.GetFiles(AppContext.BaseDirectory, "*.dll")
        .Select(Assembly.LoadFrom)
        .Where(p => p.GetName().Name.StartsWith("App.")).ToArray();

// Get all implementations of IServiceRegistrar dynamically
    var registrars = assemblies
        .SelectMany(a => a.GetTypes())
        .Where(t => typeof(IServiceRegistrar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
        .Select(Activator.CreateInstance)
        .Cast<IServiceRegistrar>();

// Call RegisterServices() on each found class
    foreach (var registrar in registrars)
    {
        registrar.RegisterServices(webApplicationBuilder.Services);
    }
}