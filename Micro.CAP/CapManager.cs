using DotNetCore.CAP.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Micro.CAP
{
    public static class CapManager
    {
        public static IServiceCollection AddCapExt(this IServiceCollection app, string rabbitconn, string sqlconn)
        {
            //注册Cap
            app.AddCap(cap =>
            {
                cap.UseRabbitMQ(rabbitconn);
                cap.UseSqlServer(sqlconn);
                cap.FailedRetryCount = 10;//重试次数
                cap.FailedRetryInterval = 30;//重试间隔
                cap.FailedThresholdCallback = (ck) =>//失败回调
                {
                    //发送回调通知人来处理
                    Console.WriteLine($"消息推送失败了:类型{ck.MessageType}消息内容:{ck.Message.GetName()}重试了:{cap.FailedRetryCount}次");
                };
            });
            return app;
        }
    }
}
