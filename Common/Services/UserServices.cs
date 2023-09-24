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
        var now = TimeManager.GetNow();
        var today = TimeManager.GetCurrentApplicationDate();
        var start = today + MasterManager.BedInStart;
        var end = today + MasterManager.BedInEnd;

        // 就寝可能な時間外だったら終了
        if (now < start || end < now) return null;

        await using var context = new SpringDbContext();

        // 最新の就寝を取得
        var bedIn = await context.Set<BedIn>()
            .Where(x => x.UserId == UserId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();

        if (bedIn != null && bedIn.ApplicationDate == today)
        {
            // 既に本日の就寝記録がある場合は何もしない
            return null;
        }

        // 就寝を記録
        bedIn = new BedIn
        {
            UserId = UserId,
            ApplicationDate = today,
        };
        context.Add(bedIn);

        await context.SaveChangesAsync();
        return bedIn;
    }

    public async Task<WakeUp?> WakeUpAsync()
    {
        var now = TimeManager.GetNow();

        await using var context = new SpringDbContext();

        // 最新の就寝を取得
        var bedIn = await context.Set<BedIn>()
            .Where(x => x.UserId == UserId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(1)
            .Include(x => x.WakeUp)
            .FirstOrDefaultAsync();

        // まだ一度も就寝してない or 起床済み or まだ就寝可能な時間中 ならば何もしない
        if (bedIn == null ||
            bedIn.WakeUp != null ||
            now < bedIn.ApplicationDate + MasterManager.BedInEnd) return null;

        var wakeUp = new WakeUp
        {
            UserId = UserId,
            BedInId = bedIn.Id,
        };

        // 早すぎ（夜更かし）、ちょうど良い、遅すぎのどれかを判定
        if (now < bedIn.ApplicationDate + MasterManager.WakeUpStart)
        {
            wakeUp.ResultType = WakeUpResultType.TooEarly;
            wakeUp.PainfulScore = MasterManager.WakeUpFailedPainfulScore;
        }
        else if (now < bedIn.ApplicationDate + MasterManager.WakeUpEnd)
        {
            wakeUp.ResultType = WakeUpResultType.Succeed;
            wakeUp.MarvelousScore = MasterManager.WakeUpMarvelousScore;
        }
        else
        {
            wakeUp.ResultType = WakeUpResultType.TooLate;
            wakeUp.PainfulScore = MasterManager.WakeUpFailedPainfulScore;
        }

        context.Add(wakeUp);

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
        var today = TimeManager.GetCurrentApplicationDate();
        if (lastLogin != null && lastLogin.ApplicationDate == today) return null;

        // ログインボーナスを付与
        var login = new Login
        {
            UserId = UserId,
            ApplicationDate = today,
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
            TargetMarvelousScore = MasterManager.ReceivePraiseMarvelousScore,
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

    /// <summary>
    /// 他人を慰める
    /// </summary>
    public async Task<Comfort?> ComfortAsync(ulong targetUserId, ulong messageId, int reactionId)
    {
        // 自分自身を慰めることはできない
        if (UserId == targetUserId) return null;

        await using var context = new SpringDbContext();

        // 既に同一条件で褒めたかどうかチェック あったら終了
        var existComfort = await context.Set<Comfort>().FirstOrDefaultAsync(comfort =>
            comfort.DiscordMessageId == messageId &&
            comfort.DiscordReactionId == reactionId &&
            comfort.UserId == UserId &&
            comfort.TargetUserId == targetUserId);
        if (existComfort != null) return null;

        // ユーザーがいなければ作成
        await AppServices.FindOrCreateUserAsync(UserId);
        await AppServices.FindOrCreateUserAsync(targetUserId);

        // 慰めたことを記録
        var comfort = new Comfort
        {
            UserId = UserId,
            DiscordMessageId = messageId,
            DiscordReactionId = reactionId,
            TargetUserId = targetUserId,
            MarvelousScore = MasterManager.SendComfortMarvelousScore,
            TargetPainfulScore = MasterManager.ReceiveComfortPainfulScore,
        };
        context.Add(comfort);

        await context.SaveChangesAsync();
        return comfort;
    }

    /// <summary>
    /// 他人を慰めたのを取り消す
    /// </summary>
    public async Task<Comfort?> CancelComfortAsync(ulong targetUserId, ulong messageId, int reactionId)
    {
        await using var context = new SpringDbContext();

        // 同一条件で慰めた記録を検索 なければ終了
        var comfort = await context.Set<Comfort>().FirstOrDefaultAsync(comfort =>
            comfort.DiscordMessageId == messageId &&
            comfort.DiscordReactionId == reactionId &&
            comfort.UserId == UserId &&
            comfort.TargetUserId == targetUserId);
        if (comfort == null) return null;

        // 慰めた記録を削除
        context.Remove(comfort);

        await context.SaveChangesAsync();
        return comfort;
    }
}