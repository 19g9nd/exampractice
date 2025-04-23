using BackgroundProducer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<RabbitService>();

var host = builder.Build();
host.Run();
