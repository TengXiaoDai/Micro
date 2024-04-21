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
        //ʧ�ܻص�,���ʼ�֪ͨ����Ա
        Console.WriteLine($"��Ϣ����ʧ�ܡ���Ϣ����:{err.MessageType} ��Ϣ����:{err.Message.GetName()} ��Ϣ���Դ���:{cap.FailedRetryCount}�Ρ�");
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

await app.UseConsolRegistry(app.Lifetime);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
