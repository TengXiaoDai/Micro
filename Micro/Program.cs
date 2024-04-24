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

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//�滻Ĭ�ϵ�IOC
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
     options.UseLazyLoadingProxies();//ʹ���ӳټ���
     options.UseSqlServer(connectionString);
     //options.UseInMemoryDatabase(new Guid().ToString());//ʹ�û���
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
