using System.Text;
using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class RankingPresenter : DiscordMessagePresenterBase
{
    public bool IsTotal { get; set; }

    protected override async Task MainAsync()
    {
        var users = await AppData.MarvelousScoreRankingAsync();

        var builder = new StringBuilder();
        var order = 1;
        foreach (var (user, score) in users)
        {
            var pointString = Math.Min(score, 999).ToString("N");
            var whiteSpace = Math.Max(0, 6 - pointString.Length * 2);
            var line =
                $"#{order:D2} - {"".PadLeft(whiteSpace, ' ')}✨{pointString}  {MentionUtils.MentionUser(user.DiscordID)}";
            builder.AppendLine(line);
            order++;
        }

        var embed = new EmbedBuilder()
            .WithColor(Color.LightOrange)
            .WithTitle($"{MarvelousScoreIcon}{MarvelousScoreName} ランキング")
            .AddField("", builder.ToString())
            .WithCurrentTimestamp()
            .Build();

        await Message.ReplyAsync(embed: embed);
    }
}