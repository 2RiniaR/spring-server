using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class RankingPresenter : DiscordMessagePresenterBase
{
    public override async Task RunAsync()
    {
        var users = await AppServices.RankingAsync();

        var ranking = users.Select((user, index) => RenderUserText(index + 1, user));
        var embed = new EmbedBuilder()
            .WithColor(Color.LightOrange)
            .WithTitle("✨えらいポイント　ランキング")
            .AddField("", string.Join("\n", ranking))
            .WithCurrentTimestamp()
            .Build();

        await Message.ReplyAsync(embed: embed);
    }

    private static string RenderUserText(int order, User user)
    {
        var pointString = Math.Min(user.TotalScore, 999).ToString("N");
        var whiteSpace = Math.Max(0, 6 - pointString.Length * 2);

        return
            $"#{order:D2} - {"".PadLeft(whiteSpace, ' ')}✨{pointString}  {MentionUtils.MentionUser(user.DiscordID)}";
    }
}