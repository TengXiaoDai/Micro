using Autofac.Extras.DynamicProxy;
using DotNetCore.CAP;
using Micro.Models;
using Micro.Respository;
using Micro.User.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuPeng.HystrixCore;

namespace Micro.User.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Intercept(typeof(AuthInterceptor))]
    public class HomeController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICapPublisher _capPublisher;
        private readonly IOrderRepository _orderRepository;
        public HomeController(IUserRepository userRepository, ICapPublisher capPublisher, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _userRepository = userRepository;
            _capPublisher = capPublisher;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("获取了------------------------------------");
            return new JsonResult(await _userRepository.GetByIdAsync(1));
        }


        [HttpGet]
        public async Task<ActionResult> Compile()
        {
            var ret = _userRepository.GetByConditionWithCompile(a => a.Id == 1);
            return new JsonResult(ret);
        }
        [HttpPost]
        public async Task<ActionResult> Lazy()
        {
            var ret = await _orderRepository.FirstOrDefualt(a => a.Id == 1);
            return new JsonResult(ret);
        }
        [HttpGet]
        public async Task<ActionResult> InStack()
        {
            try
            {
                Product product = await _productRepository.FirstOrDefualt(a => a.Id == 1);
                if (product != null && product.InStock > 0)
                {
                    product.InStock -= 1;
                    await _productRepository.UpdateAsync(product);
                    return new JsonResult(new
                    {
                        msg = $"恭喜您,抢到商品了!当前还剩库存{product.InStock}",
                        code = 200
                    });
                }
                else
                    return new JsonResult(new
                    {
                        msg = $"抱歉,已经没有库存了,当前还剩库存{product.InStock}",
                        code = 200
                    });
            }
            catch (DbUpdateConcurrencyException err) 
            {
                return new JsonResult(new
                {
                    msg = $"抱歉,已经没有库存了",
                    code = 200
                });
            }
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
