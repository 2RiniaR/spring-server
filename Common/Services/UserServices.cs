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

        var bedIn = await context.Set<BedIn>()
            .Where(x => x.UserId == UserId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
        if (bedIn is not { WakeUpId: null }) return null;

        var wakeUp = new WakeUp
        {
            UserId = UserId,
        };

        await context.SaveChangesAsync();
        return wakeUp;
    }

    /// <summary>
    /// ログイン時の処理を行う
    /// </summary>
    public async Task<Login?> LoginAsync()
    {
        // ユーザーがいなければ作成
        await AppServices.FindOrCreateUserAsync(UserId);

        await using var context = new SpringDbContext();

        // 最後にログインしたときのデータを取得
        var lastLogin = await context.Set<Login>()
            .Where(x => x.UserId == UserId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        // 本日既にログイン済みの場合はnullを返す
        var date = TimeManager.GetApplicationDate();
        if (lastLogin != null && lastLogin.ApplicationDate == date) return null;

        // ログインボーナスを付与
        var login = new Login
        {
            UserId = UserId,
            ApplicationDate = date,
            MarvelousScore = MasterManager.LoginMarvelousScore,
        };
        context.Add(login);

        await context.SaveChangesAsync();
        return login;
    }

    /// <summary>
    /// 他人を褒める
    /// </summary>
    public async Task<Praise?> PraiseAsync(ulong targetUserId, ulong messageId, int reactionId)
    {
        // 自分自身を褒めることはできない
        if (UserId == targetUserId) return null;

        await using var context = new SpringDbContext();

        // 既に同一条件で褒めたかどうかチェック あったら終了
        var existPraise = await context.Set<Praise>().FirstOrDefaultAsync(praise =>
            praise.DiscordMessageId == messageId &&
            praise.DiscordReactionId == reactionId &&
            praise.UserId == UserId &&
            praise.TargetUserId == targetUserId);
        if (existPraise != null) return null;

        // ユーザーがいなければ作成
        await AppServices.FindOrCreateUserAsync(UserId);
        await AppServices.FindOrCreateUserAsync(targetUserId);

        // 褒めたことを記録
        var praise = new Praise
        {
            UserId = UserId,
            DiscordMessageId = messageId,
            DiscordReactionId = reactionId,
            TargetUserId = targetUserId,
            MarvelousScore = MasterManager.SendPraiseMarvelousScore,
            GivenMarvelousScore = MasterManager.ReceivePraiseMarvelousScore,
        };
        context.Add(praise);

        await context.SaveChangesAsync();
        return praise;
    }

    /// <summary>
    /// 他人を褒めたのを取り消す
    /// </summary>
    public async Task<Praise?> CancelPraiseAsync(ulong targetUserId, ulong messageId, int reactionId)
    {
        await using var context = new SpringDbContext();

        // 同一条件で褒めた記録を検索 なければ終了
        var praise = await context.Set<Praise>().FirstOrDefaultAsync(praise =>
            praise.DiscordMessageId == messageId &&
            praise.DiscordReactionId == reactionId &&
            praise.UserId == UserId &&
            praise.TargetUserId == targetUserId);
        if (praise == null) return null;

        // 褒めた記録を削除
        context.Remove(praise);

        await context.SaveChangesAsync();
        return praise;
    }
}