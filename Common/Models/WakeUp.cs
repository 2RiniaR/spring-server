using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class WakeUp : ActionBase
{
    public int BedInId { get; set; }
    public BedIn? BedIn { get; set; }
}

public class WakeUpConfiguration : IEntityTypeConfiguration<WakeUp>
{
    public void Configure(EntityTypeBuilder<WakeUp> builder)
    {
    }
}