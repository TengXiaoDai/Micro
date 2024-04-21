using DotNetCore.CAP;
using Micro.Respository;
using Microsoft.AspNetCore.Mvc;
using RuPeng.HystrixCore;

namespace Micro.User.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICapPublisher _capPublisher;
        public HomeController(IUserRepository userRepository, ICapPublisher capPublisher)
        {
            _userRepository = userRepository;
            _capPublisher = capPublisher;
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return new JsonResult(await _userRepository.GetByIdAsync(1));
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="userName">登录账号</param>
        /// <param name="pwd">登录密码</param>
        /// <returns></returns>
        [HttpPost]
        [HystrixCommand(nameof(AddUser),
            EnableCircuitBreaker = true,
            ExceptionsAllowedBeforeBreaking = 3,
            MillisecondsOfBreak = 1000 * 5)]
        public async Task<IActionResult> AddUser(string name, string userName, string pwd)
        {
            Micro.Models.User user = new Micro.Models.User()
            {
                Name = name,
                Sex = true,
                UserName = userName,
                UserPwd = pwd
            };
            bool ret = await _userRepository.AddAsync(user);
            string queueName = "Micro.User.AddUser";
            _capPublisher.Publish(queueName, user);//发布给队列让其他消费者处理
            return new JsonResult(new { isOk = ret, meg = ret ? "添加成功!" : "添加失败!" });
        }
    }
}
