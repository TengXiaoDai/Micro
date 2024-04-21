using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//ע��ocelot����
builder.Services.AddOcelot().AddConsul();

//����ocelot�����ļ�
builder.Configuration.AddJsonFile("ocelot.json", false, true);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

await app.UseOcelot();

app.MapControllers();

// ���Ĭ��·��
app.MapGet("/", () => "Hello, World!");

app.Run();
