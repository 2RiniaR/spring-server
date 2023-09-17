using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class User
{
    public ulong DiscordID { get; set; }
    public string? GitHubID { get; set; }
    public IEnumerable<ActionBase> Actions { get; } = new List<ActionBase>();
    public int TotalScore => Actions.Sum(x => x.MarvelousScore);
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.DiscordID);
        builder.HasMany<ActionBase>(x => x.Actions).WithOne(x => x.User).HasForeignKey(x => x.UserId).IsRequired();
    }
}