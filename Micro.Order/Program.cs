using Micro.Comm;
using Micro.Order.Consul;
using Micro.Respository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MicroContext>(options =>
    options.UseSqlServer(connectionString)
 , ServiceLifetime.Singleton);

builder.Services.Configure<ConsulOption>(builder.Configuration.GetSection("ConsulOption"));

builder.Services.AddSingleton<IProductRepository, ProductRepository>();

var app = builder.Build();

//Consul ×¢Èë
await app.UseConsolRegistry(app.Lifetime);

// Configure the HTTP request pipeline.

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
