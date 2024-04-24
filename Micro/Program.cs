using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using DotNetCore.CAP.Messages;
using Micro.Comm;
using Micro.gRpc.Services;
using Micro.Respository;
using Micro.User.Consul;
using Micro.User.Middleware;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProtoBuf.Grpc.Server;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//替换默认的IOC
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string rabbitMqConn = builder.Configuration.GetConnectionString("RabbitMqConn");

builder.Services.AddLogging();
builder.Services.AddSingleton<ConsulBalancer>();
builder.Services.AddDbContext<MicroContext>(options =>
{
     options.UseLazyLoadingProxies();//使用延迟加载
     options.UseSqlServer(connectionString);
     //options.UseInMemoryDatabase(new Guid().ToString());//使用缓存
 }
 , ServiceLifetime.Transient);

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.Register(c => new AuthInterceptor());


});

builder.Services.AddCap(a =>
{
    a.UseSqlServer(connectionString);
    a.UseRabbitMQ(rabbitMqConn);
    a.FailedRetryCount = 10;//重试次数
    a.FailedRetryInterval = 30;//重试间隔
    a.FailedThresholdCallback = (fail) =>//错误回调,通知错误处理
    {
        //失败回调,做邮件通知程序员
        Console.WriteLine($"消息发送失败【消息类型:{fail.MessageType} 消息名称:{fail.Message.GetName()} 消息重试次数:{a.FailedRetryCount}次】");
    };
});

//Consul 配置参数映射到容器中
builder.Services.Configure<ConsulOption>(builder.Configuration.GetSection("ConsulOption"));


builder.Services.AddCodeFirstGrpc();//注入gRPC

var app = builder.Build();


app.MapGrpcService<UserService>(); //映射服务

//Consul 注入
await app.UseConsolRegistry(app.Lifetime);

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
