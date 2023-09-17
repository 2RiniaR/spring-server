using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class HelpPresenter : DiscordMessagePresenterBase
{
    public override async Task RunAsync()
    {
        var embed = new EmbedBuilder()
            .WithColor(Color.Blue)
            .WithTitle("エライさん ヘルプ")
            .WithDescription(HelpView.Text)
            .WithCurrentTimestamp()
            .Build();

        // DMでヘルプを送る
        await Message.Author.SendMessageAsync(embed: embed);
    }
}