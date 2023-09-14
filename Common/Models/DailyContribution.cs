using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class DailyContribution : ActionBase
{
    public int Count { get; set; }
}

public class DailyContributionConfiguration : IEntityTypeConfiguration<DailyContribution>
{
    public void Configure(EntityTypeBuilder<DailyContribution> builder)
    {
    }
}