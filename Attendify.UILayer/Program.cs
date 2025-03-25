using Attendify.UILayer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlicationServices(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware();

app.Run();
