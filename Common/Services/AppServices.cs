using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public static class AppServices
{
    public static async Task<IReadOnlyList<User>> RankingAsync()
    {
        await using var context = new SpringDbContext();
        var users = await context.Set<User>().Include(user => user.Actions).ToListAsync();
        return users.OrderBy(user => user.TotalScore).ToList();
    }

    public static async Task<IReadOnlyList<DailyContribution>> RecordContributionAsync()
    {
        await using var context = new SpringDbContext();

        var users = await context.Set<User>().ToListAsync();
        var contributions = users.ToDictionary(user => user, _ => 0);
        await GitHubManager.FetchContributionAsync(contributions);

        var dailyContributions = new List<DailyContribution>();
        foreach (var (user, count) in contributions)
        {
            var dailyContribution = new DailyContribution
            {
                UserId = user.DiscordID,
                Count = count,
            };
            dailyContributions.Add(dailyContribution);
        }

        context.AddRange(dailyContributions);

        await context.SaveChangesAsync();
        return dailyContributions;
    }

    public static async Task<User> FindOrCreateUserAsync(ulong id)
    {
        await using var context = new SpringDbContext();

        var user = await context.FindAsync<User>(id);
        if (user != null) return user;

        user = new User
        {
            DiscordID = id,
        };
        context.Add(user);
        await context.SaveChangesAsync();

        return user;
    }
}