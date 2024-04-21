using Micro.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Micro.Respository.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("micro_Users");
            builder.HasIndex(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
