using Discord;
using Discord.WebSocket;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class UserPresenter : DiscordMessagePresenterBase
{
    public SocketUser? TargetUser { get; set; }

    public override async Task RunAsync()
    {
        TargetUser ??= Message.Author;

        var targetUser = await AppServices.FindOrCreateUserAsync(TargetUser.Id);

        var embed = new EmbedBuilder()
            .WithColor(Color.LightOrange)
            .WithTitle(TargetUser.Username)
            .AddField("えらいポイント", targetUser.TotalScore)
            .WithCurrentTimestamp()
            .Build();
        await Message.ReplyAsync(embed: embed);
    }
}