using Ocelot.DependencyInjection;
using Ocelot.Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json",false,true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot();
builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowAnyOrigin",
    builder =>
    {
        builder.AllowAnyOrigin()
                 .AllowAnyHeader()
                 .AllowAnyMethod();
    });
});
var app = builder.Build();
app.UseCors("AllowAnyOrigin");
app.UseSwagger();
app.UseSwaggerUI();
app.UseOcelot().Wait();

app.UseHttpsRedirection();

app.Run();
