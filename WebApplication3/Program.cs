using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Identity.Web;
using Serilog;
using WebApplication3.GlobalExceptionHandler;
using WebApplication3.Controllers;
using WebApplication3.Logger;

var builder = WebApplication.CreateBuilder(args);

// For building logs in your application 


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddWindowsService();
builder.Services.AddControllers()
    .AddApplicationPart(typeof(DemoController).Assembly);
/*
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });

    serverOptions.ListenLocalhost(5118);  // Configure the HTTP endpoint
    /*
    serverOptions.ListenLocalhost(7071, listenOptions =>
    {
        listenOptions.UseHttps();  // Configure the HTTPS endpoint
    });

}); */


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Host.UseSerilog((context, configuration) => 
   configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseSerilogRequestLogging();


app.UseMiddleware<LogTracer>();

app.UseMiddleware<GlobalExceptionHandler>();

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
