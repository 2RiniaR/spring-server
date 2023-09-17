using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class BedIn : ActionBase
{
    public int? WakeUpId { get; set; }
    public WakeUp? WakeUp { get; set; }

    /// <summary>
    /// どの日分の就寝か
    /// </summary>
    public DateTime ApplicationDate { get; set; }
}

public class BedInConfiguration : IEntityTypeConfiguration<BedIn>
{
    public void Configure(EntityTypeBuilder<BedIn> builder)
    {
        builder.HasOne(x => x.WakeUp).WithOne(x => x.BedIn).HasForeignKey<WakeUp>(x => x.BedInId).IsRequired(false);
    }
}