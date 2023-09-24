using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class Praise : ActionBase
{
    public ulong DiscordMessageId { get; set; }
    public int DiscordReactionId { get; set; }
    public ulong TargetUserId { get; set; }
    public User? TargetUser { get; set; }
    public int TargetMarvelousScore { get; set; }
}

public class PraiseConfiguration : IEntityTypeConfiguration<Praise>
{
    public void Configure(EntityTypeBuilder<Praise> builder)
    {
    }
}