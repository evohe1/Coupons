using AmazonGrpcService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory 
};

var builder = WebApplication.CreateBuilder(options);



builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5006, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        //if (builder.Configuration.GetSection("SSL")["sertificateName"].Trim() != "")
        //    listenOptions.UseHttps(Path.Combine(AppContext.BaseDirectory, "cfg", builder.Configuration.GetSection("SSL")["sertificateName"]), builder.Configuration.GetSection("SSL")["password"]);

    });
});

builder.Host.UseWindowsService();
//builder.Services.AddHostedService<WindowsBackgroundService>();


// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();

app.UseRouting();
app.UseGrpcWeb(); // Must be added between UseRouting and UseEndpoints
app.UseCors();

app.UseEndpoints(endpoints =>
{
    app.MapGrpcService<ProductService>().RequireCors("AllowAll").EnableGrpcWeb();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
});



app.Run();
