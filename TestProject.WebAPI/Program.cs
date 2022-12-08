using Microsoft.EntityFrameworkCore;
using TestProject.Data.Context;
using TestProject.WebAPI.Extension;
using TestProject.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

//To enable microsoft logging
builder.Host.ConfigureLogging((hostingContext, logging) =>
 {
	 logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
	 logging.AddConsole();
	 logging.AddDebug();
	 logging.AddEventSourceLogger();
 });

//Environment based appsetting config
builder.Configuration
	.SetBasePath(builder.Environment.ContentRootPath)
	.AddJsonFile("appsettings.json", true, true)
	.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
	.AddEnvironmentVariables();

builder.Services.AddLogging();

//Register Filters
builder.Services.AddMvcCore(options =>
{
	options.Filters.Add(typeof(RequestModelValidationFilter));
});

//Configure DependencyInjections
builder.AddServiceInjection();

//Configure Database
builder.Services.AddDbContext<AltimetrikDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("AltimetrikConnecton")));

//Configure Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Configure API Versioning
builder.AddVersioning();

//Jwt Authorization
//builder.AddJwtToken();

//Enable Cros
builder.Services.AddCors();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(
	c =>
	{
		c.AllowAnyHeader();
		c.AllowAnyMethod();
		c.AllowAnyOrigin();
	});

app.UseMiddleware<ExceptionMiddleware>();
//app.UseJwtToken();

app.MapControllers();

app.Run();