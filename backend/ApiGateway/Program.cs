var builder = WebApplication.CreateBuilder(args);

// YARP czyta konfigurację routingu z appsettings.json z sekcji "ReverseProxy"
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.UseCors();

app.MapGet("/health", () => Results.Ok("healthy"));

app.MapReverseProxy();

app.Run();