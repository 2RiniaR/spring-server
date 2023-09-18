using Discord;
using Microsoft.EntityFrameworkCore;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class UserPresenter : DiscordMessagePresenterBase
{
    public IUser? TargetUser { get; set; }
    public bool IsTotal { get; set; }

    protected override async Task MainAsync()
    {
        TargetUser ??= Message.Author;

        // ユーザーがいなければ作る
        await AppServices.FindOrCreateUserAsync(TargetUser.Id);

        // 集計開始日時
        var periodStart = IsTotal ? DateTime.MinValue : TimeManager.GetCurrentApplicationWeekStart();
        var periodEnd = IsTotal ? DateTime.MaxValue : TimeManager.GetCurrentApplicationWeekEnd();

        var stats = new List<(string, int)>
        {
            (
                "褒めた回数",
                await ApplicationData<Praise>()
                    .Where(x => x.UserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
            (
                "褒められた回数",
                await ApplicationData<Praise>()
                    .Where(x => x.TargetUserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
            (
                "慰めた回数",
                await ApplicationData<Comfort>()
                    .Where(x => x.UserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
            (
                "慰められた回数",
                await ApplicationData<Comfort>()
                    .Where(x => x.TargetUserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
            (
                "生きた日数",
                await ApplicationData<Login>()
                    .Where(x => x.UserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
            (
                "生活リズムを守った日数",
                await ApplicationData<WakeUp>()
                    .Where(x => x.UserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
            (
                "GitHubで活動した日数",
                await ApplicationData<DailyContribution>()
                    .Where(x => x.UserId == TargetUser.Id && x.CreatedAt >= periodStart)
                    .CountAsync()
            ),
        };

        var totalPeriodText = IsTotal ? "全期間" : $"{DateTimeText(periodStart)} ～ {DateTimeText(periodEnd)}";

        var embed = new EmbedBuilder()
            .WithColor(Color.LightOrange)
            .WithTitle(UserNameText(TargetUser))
            .WithThumbnailUrl(TargetUser.GetAvatarUrl() ?? TargetUser.GetDefaultAvatarUrl())
            .WithDescription($"集計期間: `{totalPeriodText}`")
            .AddField($"{MarvelousScoreName}", MarvelousScoreText(1), true)
            .AddField($"{PainfulScoreName}", PainfulScoreText(1), true)
            .AddField("統計", $"```{IntTableText(stats)}```")
            .WithCurrentTimestamp()
            .Build();

        await Message.ReplyAsync(embed: embed);
    }
}