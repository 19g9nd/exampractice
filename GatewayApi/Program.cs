using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddJsonFile("ocelot.json",false,true);
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
await app.UseOcelot();
app.UseHttpsRedirection();

app.Run();

