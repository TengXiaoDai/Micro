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
    a.FailedRetryCount = 10;//���Դ���
    a.FailedRetryInterval = 30;//���Լ��
    a.FailedThresholdCallback = (fail) =>//����ص�,֪ͨ������
    {
        //ʧ�ܻص�,���ʼ�֪ͨ����Ա
        Console.WriteLine($"��Ϣ����ʧ�ܡ���Ϣ����:{fail.MessageType} ��Ϣ����:{fail.Message.GetName()} ��Ϣ���Դ���:{a.FailedRetryCount}�Ρ�");
    };
});

//Consul ���ò���ӳ�䵽������
builder.Services.Configure<ConsulOption>(builder.Configuration.GetSection("ConsulOption"));


builder.Services.AddCodeFirstGrpc();//ע��gRPC

var app = builder.Build();

app.MapGrpcService<UserService>(); //ӳ�����

//Consul ע��
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
