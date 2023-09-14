using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RineaR.Spring.Common;
using RineaR.Spring.Events;

namespace RineaR.Spring;

public static class DiscordEntry
{
    public static void RegisterEvents()
    {
        DiscordManager.RegisterCommands<CommandDefine>();
        DiscordManager.Client.MessageReceived += OnMessageReceived;
        DiscordManager.Client.ReactionAdded += OnReactionAdded;
        DiscordManager.Client.ReactionRemoved += OnReactionRemoved;
    }

    private static async Task OnMessageReceived(SocketMessage message)
    {
        // botは弾く
        if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot) return;

        DiscordManager.ExecuteMatchedCommand(userMessage, MasterManager.DiscordCommandPrefix);
        DiscordManager.Execute<GreetPresenter>(userMessage);
        if (userMessage.Content == "おやすみ") DiscordManager.Execute<BedInPresenter>(userMessage);
        if (userMessage.MentionedUsers.Contains(DiscordManager.Client.CurrentUser))
            DiscordManager.Execute<HelpPresenter>(userMessage);
    }

    private static async Task OnReactionAdded(Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        var reactedUser = await DiscordManager.Client.GetUserAsync(reaction.UserId);
        var messageAuthor = (await message.GetOrDownloadAsync()).Author;

        // botは弾く
        if (reactedUser.IsBot || messageAuthor.IsBot) return;

        if (reaction.Emote.Name == "👍") DiscordManager.Execute<PraisePresenter>(reaction);
    }

    private static async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        var reactedUser = await DiscordManager.Client.GetUserAsync(reaction.UserId);
        var messageAuthor = (await message.GetOrDownloadAsync()).Author;

        // botは弾く
        if (reactedUser.IsBot || messageAuthor.IsBot) return;

        if (reaction.Emote.Name == "👍") DiscordManager.Execute<CancelPraisePresenter>(reaction);
    }

    private class CommandDefine : ModuleBase<SocketCommandContext>
    {
        [Command("user")]
        public void User()
        {
            DiscordManager.Execute<UserPresenter>(Context.Message);
        }

        [Command("me")]
        public void Me()
        {
            DiscordManager.Execute<UserPresenter>(Context.Message);
        }

        [Command("ranking")]
        public void Ranking()
        {
            DiscordManager.Execute<UserPresenter>(Context.Message);
        }

        [Command("help")]
        public void Help()
        {
            DiscordManager.Execute<UserPresenter>(Context.Message);
        }
    }
}