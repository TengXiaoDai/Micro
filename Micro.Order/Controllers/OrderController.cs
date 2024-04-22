using Micro.Models;
using Micro.Respository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public OrderController(IProductRepository productRepository) 
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        public async Task MakOrder() 
        {
            #region 分布式锁进行库存秒杀
            using Micro.Redis.RedisLock redis= new Micro.Redis.RedisLock();
            Product product = await _productRepository.GetByIdAsync(1);
            try
            {
                redis.Lock();
                if (product != null && product.InStock > 0)
                {
                    product.InStock -= 1;
                    bool isSuccess = await _productRepository.UpdateAsync(product);
                    Console.WriteLine(isSuccess ? "恭喜您,秒杀成功!" : "秒杀失败!");
                }
                else
                    Console.WriteLine("秒杀失败,已无库存!");
            }
            finally 
            {
                redis.Unlock();
            }
            #endregion
        }
    }
}
