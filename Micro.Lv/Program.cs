using DotNetCore.CAP.Messages;
using Micro.Comm;
using Micro.Lv.Consul;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string sqlconn = builder.Configuration.GetConnectionString("DefaultConnection");
string rabbitMq = builder.Configuration.GetConnectionString("RabbitMqConn");

builder.Services.Configure<ConsulOption>(builder.Configuration.GetSection("ConsulOption"));

builder.Services.AddCap(cap =>
{
    cap.UseSqlServer(sqlconn);
    cap.UseRabbitMQ(rabbitMq);
    cap.FailedRetryCount = 10;
    cap.FailedRetryInterval = 30;
    cap.FailedThresholdCallback = (err) =>
    {
        //失败回调,做邮件通知程序员
        Console.WriteLine($"消息发送失败【消息类型:{err.MessageType} 消息名称:{err.Message.GetName()} 消息重试次数:{cap.FailedRetryCount}次】");
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

await app.UseConsolRegistry(app.Lifetime);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
