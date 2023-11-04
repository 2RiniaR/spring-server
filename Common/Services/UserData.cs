using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public class UserData
{
    public static UserData As(ulong userId)
    {
        return new UserData(userId);
    }

    private UserData(ulong userId)
    {
        UserId = userId;
    }

    public ulong UserId { get; }

    public async Task<int> MarvelousScoreAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();

        var actions = await context.Set<ActionBase>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart)
            .ToListAsync();
        var praises = await context.Set<Praise>()
            .Where(x => x.TargetUserId == UserId && x.CreatedAt >= periodStart)
            .ToListAsync();

        return User.MarvelousScore(actions, praises);
    }

    public async Task<int> PainfulScoreAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();

        var actions = await context.Set<ActionBase>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart)
            .ToListAsync();
        var comforts = await context.Set<Comfort>()
            .Where(x => x.TargetUserId == UserId && x.CreatedAt >= periodStart)
            .ToListAsync();

        return User.PainfulScore(actions, comforts);
    }

    public async Task<int> SendPraiseCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<Praise>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart)
            .CountAsync();
    }

    public async Task<int> ReceivePraiseCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<Praise>()
            .Where(x => x.TargetUserId == UserId && x.CreatedAt >= periodStart)
            .CountAsync();
    }

    public async Task<int> SendComfortCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<Comfort>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart)
            .CountAsync();
    }

    public async Task<int> ReceiveComfortCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<Comfort>()
            .Where(x => x.TargetUserId == UserId && x.CreatedAt >= periodStart)
            .CountAsync();
    }

    public async Task<int> LoginCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<Login>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart)
            .CountAsync();
    }

    public async Task<int> WakeUpCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<WakeUp>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart)
            .CountAsync();
    }

    public async Task<int> DailyContributionCountAsync(DateTime? periodStart = null)
    {
        periodStart ??= DateTime.MinValue;

        await using var context = new SpringDbContext();
        return await context.Set<DailyContribution>()
            .Where(x => x.UserId == UserId && x.CreatedAt >= periodStart && x.Count > 0)
            .CountAsync();
    }
}