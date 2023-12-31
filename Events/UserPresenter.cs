﻿using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class UserPresenter : DiscordMessagePresenterBase
{
    public IUser? TargetUser { get; set; }
    public bool IsTotal { get; set; }

    protected override async Task MainAsync()
    {
        if (TargetUser == null || TargetUser.IsBot)
        {
            await Message.ReplyAsync(embed: ErrorEmbed("ユーザーが見つかりませんでした。"));
            return;
        }

        // ユーザーがいなければ作る
        await AppServices.FindOrCreateUserAsync(TargetUser.Id);

        // 集計開始日時
        var periodStart = IsTotal ? DateTime.MinValue : TimeManager.GetCurrentApplicationWeekStart();
        var periodEnd = IsTotal ? DateTime.MaxValue : TimeManager.GetCurrentApplicationWeekEnd();

        var marvelousScore = await UserData.As(TargetUser.Id).MarvelousScoreAsync(periodStart);
        var painfulScore = await UserData.As(TargetUser.Id).PainfulScoreAsync(periodStart);
        var stats = new List<(string, int)>
        {
            ("褒めた回数",
                await UserData.As(TargetUser.Id).SendPraiseCountAsync(periodStart)),
            ("褒められた回数",
                await UserData.As(TargetUser.Id).ReceivePraiseCountAsync(periodStart)),
            ("慰めた回数",
                await UserData.As(TargetUser.Id).SendComfortCountAsync(periodStart)),
            ("慰められた回数",
                await UserData.As(TargetUser.Id).ReceiveComfortCountAsync(periodStart)),
            ("生きた日数",
                await UserData.As(TargetUser.Id).LoginCountAsync(periodStart)),
            ("生活リズムを守った日数",
                await UserData.As(TargetUser.Id).WakeUpCountAsync(periodStart)),
            ("GitHubで活動した日数",
                await UserData.As(TargetUser.Id).DailyContributionCountAsync(periodStart)),
        };

        var totalPeriodText = IsTotal ? "全期間" : $"{Format.DateTime(periodStart)} ～ {Format.DateTime(periodEnd)}";

        var embed = new EmbedBuilder()
            .WithColor(Color.LightOrange)
            .WithTitle(Format.UserName(TargetUser))
            .WithThumbnailUrl(TargetUser.GetAvatarUrl() ?? TargetUser.GetDefaultAvatarUrl())
            .WithDescription($"集計期間: {Format.Code(totalPeriodText)}")
            .AddField($"{Format.MarvelousScoreName}", Format.MarvelousScore(marvelousScore), true)
            .AddField($"{Format.PainfulScoreName}", Format.PainfulScore(painfulScore), true)
            .AddField("統計", $"```{Format.Table(stats)}```")
            .WithCurrentTimestamp()
            .Build();

        await Message.ReplyAsync(embed: embed);
    }
}