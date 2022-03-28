using mercury_api_gateway.Constants;
using mercury_api_gateway.Extensions;
using mercury_api_gateway.Models.settings;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("ocelot.json");
});

builder.Services.AddOcelot();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var auth0Settings = builder.Configuration.GetSection(Application.Settings.Auth0).Get<Auth0Settings>();
builder.Services.ConfigureAuthentication(auth0Settings, builder.Environment);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerForOcelotUI(opt => {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
    app.UseOcelot().Wait();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
