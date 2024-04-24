using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Micro.Respository.Ext
{
    public class SnowFlowGenerator : ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;

        protected override object? NextValue(EntityEntry entry)
        {
            //生成雪花Id
           return new SnowflakeIdGenerator(1,2).GenerateId();
        }
    }
}
