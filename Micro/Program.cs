using DotNetCore.CAP.Messages;
using Micro.Comm;
using Micro.gRpc.Services;
using Micro.Respository;
using Micro.User.Consul;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseSqlServer(connectionString)
 , ServiceLifetime.Singleton);

builder.Services.AddSingleton<IUserRepository, UserRepository>();

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
