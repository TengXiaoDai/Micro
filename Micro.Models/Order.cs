using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Models
{
    public class Order
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual User User { get; set; } //懒加载
    }
}
