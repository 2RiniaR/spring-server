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

        // ログイン
        await DiscordManager.ExecuteAsync<LoginPresenter>(userMessage);

        // コマンド実行
        await DiscordManager.ExecuteMatchedCommandAsync(userMessage, MasterManager.DiscordCommandPrefix);

        // 起床
        await DiscordManager.ExecuteAsync<GreetPresenter>(userMessage);

        // 特定のメッセージの時、就寝
        if (userMessage.Content == MasterManager.BedInPhrase)
            await DiscordManager.ExecuteAsync<BedInPresenter>(userMessage);

        // メンションされたとき、ヘルプを表示
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

        // 特定のリアクションをしたとき、褒める
        if (MasterManager.PraiseEmotes.Contains(reaction.Emote.Name))
            await DiscordManager.ExecuteAsync<PraisePresenter>(reaction, messageAuthor);

        // 特定のリアクションをしたとき、慰める
        if (MasterManager.ComfortEmotes.Contains(reaction.Emote.Name))
            await DiscordManager.ExecuteAsync<ComfortPresenter>(reaction, messageAuthor);
    }

    private static async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> message,
        Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
    {
        var reactedUser = await DiscordManager.Client.GetUserAsync(reaction.UserId);
        var messageAuthor = (await message.GetOrDownloadAsync()).Author;

        // botは弾く
        if (reactedUser.IsBot || messageAuthor.IsBot) return;

        // 特定のリアクションを外したとき、褒めたのを取り消す
        if (MasterManager.PraiseEmotes.Contains(reaction.Emote.Name))
            await DiscordManager.ExecuteAsync<CancelPraisePresenter>(reaction, messageAuthor);

        // 特定のリアクションを外したとき、慰めたのを取り消す
        if (MasterManager.ComfortEmotes.Contains(reaction.Emote.Name))
            await DiscordManager.ExecuteAsync<CancelComfortPresenter>(reaction, messageAuthor);
    }

    /// <summary>
    /// 各コマンドの定義
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local")]
    private class CommandDefine : ModuleBase<SocketCommandContext>
    {
        [Command("user")]
        public async Task User(SocketUser? user = null, bool total = false)
        {
            await DiscordManager.ExecuteAsync<UserPresenter>(Context.Message, presenter =>
            {
                presenter.TargetUser = user;
                presenter.IsTotal = total;
            });
        }

        [Command("me")]
        public async Task Me(bool total = false)
        {
            await DiscordManager.ExecuteAsync<UserPresenter>(Context.Message,
                presenter => { presenter.IsTotal = total; });
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