using DotNetCore.CAP;
using Micro.Models;
using Micro.Respository;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Lv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapController : ControllerBase
    {
        private readonly ICapPublisher _capPublisher;
        private readonly IUserRepository _userRepository;
        public CapController(ICapPublisher capPublisher, IUserRepository userRepository)
        {
            _capPublisher = capPublisher;
            _userRepository= userRepository;
        }

        //订阅--消费者
        [NonAction] //必须，防止被转成方法调用
        [CapSubscribe("Micro.User.AddUser", Group = "LvService.Distribute")]
        public async void LvDetail(User user, [FromCap] CapHeader header)
        {
            Console.WriteLine("收到订阅信息!");
            user.Lv = 1;//注册升级成为水手
            await  _userRepository.UpdateAsync(user);
        }
    }
}
