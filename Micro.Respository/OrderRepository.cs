using Micro.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Respository
{
    public class OrderRepository : BaseRepository<Order>,IOrderRepository
    {
        public OrderRepository(MicroContext context) : base(context)
        {

        }

        public IEnumerable<Order> NoLazyGetCondition(Func<Order, bool> where)
        {
            return base._context.Set<Order>().AsNoTracking() //关闭跟踪
                .Include(a=>a.User).Where(where).ToList();
        }
    }
}
