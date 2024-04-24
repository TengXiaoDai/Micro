using Micro.Models;
using Micro.Respository.Ext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Respository.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("micro_Order").HasOne(a=>a.User);
            builder.Property(a => a.Id).ValueGeneratedNever();
            builder.Property(a => a.Id).HasValueGenerator<SnowFlowGenerator>();

        }
    }
}
