using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RineaR.Spring.Common;

public class User
{
    public ulong DiscordID { get; set; }
    public string? GitHubID { get; set; }

    public static int MarvelousScore(IEnumerable<ActionBase> actions, IEnumerable<Praise> praises)
    {
        var actionScore = actions.Sum(x => x.MarvelousScore);
        var praiseScore = praises.Sum(x => x.TargetMarvelousScore);
        return actionScore + praiseScore;
    }

    public static int PainfulScore(IEnumerable<ActionBase> actions, IEnumerable<Comfort> comforts)
    {
        var actionScore = actions.Sum(x => x.PainfulScore);
        var praiseScore = comforts.Sum(x => x.TargetPainfulScore);
        return actionScore + praiseScore;
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.DiscordID);
    }
}