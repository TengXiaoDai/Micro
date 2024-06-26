﻿using Micro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Micro.Respository.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("micro_Product");
            builder.Property(a => a.Version).IsRowVersion();//乐观锁，防止库存卖超
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
