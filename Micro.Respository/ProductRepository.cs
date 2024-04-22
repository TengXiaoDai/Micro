using Micro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Respository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(MicroContext context) : base(context)
        {
        }
    }
}
