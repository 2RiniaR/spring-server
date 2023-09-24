using System.Text;
using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class RankingPresenter : DiscordMessagePresenterBase
{
    public bool IsTotal { get; set; }

    protected override async Task MainAsync()
    {
        // 集計開始日時
        var periodStart = IsTotal ? DateTime.MinValue : TimeManager.GetCurrentApplicationWeekStart();
        var periodEnd = IsTotal ? DateTime.MaxValue : TimeManager.GetCurrentApplicationWeekEnd();

        var users = await AppData.MarvelousScoreRankingAsync(periodStart, 8);

        var builder = new StringBuilder();
        var order = 1;
        foreach (var (user, score) in users)
        {
            var scoreText = Format.MarvelousScore(Math.Min(score, 999), true);
            var whiteSpace = Math.Max(0, 8 - scoreText.Length * 2);
            var line =
                Format.Code($"#{order:D2} - {"".PadLeft(whiteSpace, ' ')}{scoreText}") + "  " +
                MentionUtils.MentionUser(user.DiscordID);
            builder.AppendLine(line);
            order++;
        }

        var totalPeriodText = IsTotal ? "全期間" : $"{Format.DateTime(periodStart)} ～ {Format.DateTime(periodEnd)}";

        var embed = new EmbedBuilder()
            .WithColor(Color.LightOrange)
            .WithTitle($"{Format.MarvelousScoreIcon}{Format.MarvelousScoreName}")
            .WithDescription($"集計期間: {Format.Code(totalPeriodText)}")
            .AddField("ランキング", builder.ToString())
            .WithCurrentTimestamp()
            .Build();

        await Message.ReplyAsync(embed: embed);
    }
}