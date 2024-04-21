using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//注入ocelot网关
builder.Services.AddOcelot().AddConsul();

//加载ocelot配置文件
builder.Configuration.AddJsonFile("ocelot.json", false, true);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

await app.UseOcelot();

app.MapControllers();

// 添加默认路由
app.MapGet("/", () => "Hello, World!");

app.Run();
