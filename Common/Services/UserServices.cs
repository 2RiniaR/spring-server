using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public class UserServices
{
    public static UserServices As(ulong userId)
    {
        return new UserServices(userId);
    }

    private UserServices(ulong userId)
    {
        UserId = userId;
    }

    public ulong UserId { get; }

    public async Task<BedIn?> BedInAsync()
    {
        await using var context = new SpringDbContext();

        var now = TimeManager.GetNow();
        var bedIn = new BedIn
        {
            UserId = UserId,
        };
        context.Add(bedIn);

        await context.SaveChangesAsync();
        return bedIn;
    }

    public async Task<WakeUp?> WakeUpAsync()
    {
        await using var context = new SpringDbContext();

        var bedIn = await context.FindAsync<BedIn>();
        if (bedIn is not { WakeUpId: null }) return null;

        var wakeUp = new WakeUp
        {
            UserId = UserId,
        };

        await context.SaveChangesAsync();
        return wakeUp;
    }

    public async Task<Login?> LoginAsync()
    {
        await AppServices.FindOrCreateUserAsync(UserId);
        await using var context = new SpringDbContext();
        var lastLogin = await context.Set<Login>()
            .Where(x => x.UserId == UserId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        // 既にログイン済みの場合はnullを返す
        if (lastLogin != null &&
            lastLogin.CreatedAt.NextTime(MasterManager.DailyReset) ==
            TimeManager.GetNow().NextTime(MasterManager.DailyReset))
            return null;

        var login = new Login { UserId = UserId, Score = MasterManager.LoginScore };
        context.Add(login);
        await context.SaveChangesAsync();
        return login;
    }

    public async Task<Praise?> PraiseAsync(ulong targetUserId, ulong messageId, int reactionId)
    {
        // 自分自身を褒めることはできない
        if (UserId == targetUserId) return null;

        await using var context = new SpringDbContext();

        var existPraise = await context.Set<Praise>().FirstOrDefaultAsync(praise =>
            praise.DiscordMessageId == messageId &&
            praise.DiscordReactionId == reactionId &&
            praise.UserId == UserId &&
            praise.TargetUserId == targetUserId);
        if (existPraise == null) return null;

        var praise = new Praise
        {
            UserId = UserId,
            DiscordMessageId = messageId,
            DiscordReactionId = reactionId,
            TargetUserId = targetUserId,
        };
        context.Add(praise);

        await context.SaveChangesAsync();
        return praise;
    }

    public async Task<Praise?> CancelPraiseAsync(ulong targetUserId, ulong messageId, int reactionId)
    {
        await using var context = new SpringDbContext();

        var existPraise = await context.Set<Praise>().FirstOrDefaultAsync(praise =>
            praise.DiscordMessageId == messageId &&
            praise.DiscordReactionId == reactionId &&
            praise.UserId == UserId &&
            praise.TargetUserId == targetUserId);
        if (existPraise == null) return null;

        context.Remove(existPraise);

        await context.SaveChangesAsync();
        return existPraise;
    }
}