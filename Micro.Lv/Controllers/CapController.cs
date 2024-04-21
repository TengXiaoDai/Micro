using DotNetCore.CAP;
using Micro.Models;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Lv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapController : ControllerBase
    {
        private readonly ICapPublisher _capPublisher;
        public CapController(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        //订阅--消费者
        [NonAction] //必须，防止被转成方法调用
        [CapSubscribe("Micro.User.AddUser", Group = "LvService.Distribute")]
        public void LvDetail(User user, [FromCap] CapHeader header)
        {
            Console.WriteLine("收到订阅信息!");
        }
    }
}
