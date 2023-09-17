using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class SchedulerJobState
{
    public string Id { get; set; }
    public DateTime LastRunTime { get; set; }
}

public class SchedulerJobStateConfiguration : IEntityTypeConfiguration<SchedulerJobState>
{
    public void Configure(EntityTypeBuilder<SchedulerJobState> builder)
    {
        builder.HasKey(x => x.Id);
    }
}