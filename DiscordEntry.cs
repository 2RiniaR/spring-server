using System.Diagnostics.CodeAnalysis;
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

        await DiscordManager.ExecuteMatchedCommandAsync(userMessage, MasterManager.DiscordCommandPrefix);
        await DiscordManager.ExecuteAsync<LoginPresenter>(userMessage);
        await DiscordManager.ExecuteAsync<GreetPresenter>(userMessage);
        if (userMessage.Content == "おやすみ") await DiscordManager.ExecuteAsync<BedInPresenter>(userMessage);
        if (userMessage.IsMentioned(DiscordManager.Client.CurrentUser))
            await DiscordManager.ExecuteAsync<HelpPresenter>(userMessage);
    }

    private static async Task OnReactionAdded(Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        var reactedUser = await DiscordManager.Client.GetUserAsync(reaction.UserId);
        var messageAuthor = (await message.GetOrDownloadAsync()).Author;

        // botは弾く
        if (reactedUser.IsBot || messageAuthor.IsBot) return;

        if (reaction.Emote.Name == "👍") await DiscordManager.ExecuteAsync<PraisePresenter>(reaction);
    }

    private static async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        var reactedUser = await DiscordManager.Client.GetUserAsync(reaction.UserId);
        var messageAuthor = (await message.GetOrDownloadAsync()).Author;

        // botは弾く
        if (reactedUser.IsBot || messageAuthor.IsBot) return;

        if (reaction.Emote.Name == "👍") await DiscordManager.ExecuteAsync<CancelPraisePresenter>(reaction);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    private class CommandDefine : ModuleBase<SocketCommandContext>
    {
        [Command("user")]
        public async Task User(SocketUser user)
        {
            await DiscordManager.ExecuteAsync<UserPresenter>(Context.Message, presenter => presenter.TargetUser = user);
        }

        [Command("me")]
        public async Task Me()
        {
            await DiscordManager.ExecuteAsync<UserPresenter>(Context.Message);
        }

        [Command("ranking")]
        public async Task Ranking()
        {
            await DiscordManager.ExecuteAsync<RankingPresenter>(Context.Message);
        }

        [Command("help")]
        public async Task Help()
        {
            await DiscordManager.ExecuteAsync<HelpPresenter>(Context.Message);
        }
    }
}