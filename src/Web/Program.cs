using Microsoft.AspNetCore.Mvc.Controllers;
using Web;
using Web.Settings;
using Web.Middleware;
using Domain.Repositories;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1"}));

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddSingleton<IControllerActivator, ControllerActivator>();
builder.Services.AddSingleton<IDbConfiguration, DbConfiguration>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
