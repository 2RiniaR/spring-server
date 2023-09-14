using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class BedIn : ActionBase
{
    public int? WakeUpId { get; set; }
    public WakeUp? WakeUp { get; set; }
    public DateTime MustWakeUpSince => CreatedAt.NextTime(MasterManager.WakeUpStart);
    public DateTime MustWakeUpUntil => CreatedAt.NextTime(MasterManager.WakeUpEnd);
}

public class BedInConfiguration : IEntityTypeConfiguration<BedIn>
{
    public void Configure(EntityTypeBuilder<BedIn> builder)
    {
        builder.HasOne(x => x.WakeUp).WithOne(x => x.BedIn).HasForeignKey<WakeUp>(x => x.BedInId).IsRequired(false);
    }
}