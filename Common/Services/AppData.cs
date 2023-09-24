using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public static class AppData
{
    public static async Task<IReadOnlyList<(User user, int score)>> MarvelousScoreRankingAsync(
        DateTime? periodStart = null, int count = int.MaxValue)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();

        var users = await context.Set<User>().ToListAsync();
        var actions = await context.Set<ActionBase>().Where(x => x.CreatedAt >= periodStart).ToListAsync();
        var actionsLookup = actions.ToLookup(x => x.UserId);
        var praises = await context.Set<Praise>().Where(x => x.CreatedAt >= periodStart).ToListAsync();
        var praisesLookup = praises.ToLookup(x => x.TargetUserId);

        return users.Select(user =>
                (user, score: User.MarvelousScore(actionsLookup[user.DiscordID], praisesLookup[user.DiscordID])))
            .OrderBy(x => x.score)
            .Take(count)
            .ToList();
    }

    public static async Task<IReadOnlyList<(User user, int score)>> PainfulScoreRankingAsync(
        DateTime? periodStart = null, int count = int.MaxValue)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();

        var users = await context.Set<User>().ToListAsync();
        var actions = await context.Set<ActionBase>().Where(x => x.CreatedAt >= periodStart).ToListAsync();
        var actionsLookup = actions.ToLookup(x => x.UserId);
        var comforts = await context.Set<Comfort>().Where(x => x.CreatedAt >= periodStart).ToListAsync();
        var comfortsLookup = comforts.ToLookup(x => x.TargetUserId);

        return users.Select(user =>
                (user, score: User.PainfulScore(actionsLookup[user.DiscordID], comfortsLookup[user.DiscordID])))
            .OrderBy(x => x.score)
            .Take(count)
            .ToList();
    }
}