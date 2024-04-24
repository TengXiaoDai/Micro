using Micro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Respository
{
    public interface IOrderRepository:IBaseRepository<Order>
    {
        IEnumerable<Order> NoLazyGetCondition(Func<Order, bool> where);
    }
}
