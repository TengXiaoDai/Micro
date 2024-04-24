using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Models
{
    public class Product
    {
        public int Id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public long InStock { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public byte[] Version { get; set; }
    }
}
