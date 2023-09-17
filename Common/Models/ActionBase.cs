using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public abstract class ActionBase
{
    public int Id { get; set; }
    public ulong UserId { get; set; }
    public User? User { get; set; }
    public int MarvelousScore { get; set; }
    public int PainfulScore { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ActionBaseConfiguration : IEntityTypeConfiguration<ActionBase>
{
    public void Configure(EntityTypeBuilder<ActionBase> builder)
    {
        builder.HasDiscriminator<string>("ActionType")
            .HasValue<BedIn>("BedIn")
            .HasValue<WakeUp>("WakeUp")
            .HasValue<Login>("Login")
            .HasValue<Praise>("Praise")
            .HasValue<Comfort>("Comfort")
            .HasValue<DailyContribution>("DailyContribution");

        builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd();
    }
}