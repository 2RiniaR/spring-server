using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class WakeUp : ActionBase
{
    public int BedInId { get; set; }
    public BedIn? BedIn { get; set; }
    public WakeUpResultType ResultType { get; set; }
}

public enum WakeUpResultType
{
    Succeed,
    TooEarly,
    TooLate,
}

public class WakeUpConfiguration : IEntityTypeConfiguration<WakeUp>
{
    public void Configure(EntityTypeBuilder<WakeUp> builder)
    {
    }
}