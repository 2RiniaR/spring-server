using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class Comfort : ActionBase
{
    public ulong DiscordMessageId { get; set; }
    public int DiscordReactionId { get; set; }
    public ulong TargetUserId { get; set; }
    public User? TargetUser { get; set; }
    public int GivenPainfulScore { get; set; }
}

public class ComfortConfiguration : IEntityTypeConfiguration<Comfort>
{
    public void Configure(EntityTypeBuilder<Comfort> builder)
    {
    }
}