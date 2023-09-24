using CommandLine;
using CommandLine.Text;
using Discord;
using Discord.WebSocket;
using RineaR.Spring.Common;
using RineaR.Spring.Events;

namespace RineaR.Spring;

public static class DiscordEntry
{
    public static void RegisterEvents()
    {
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
        await CommandDefine.RunAsync(userMessage);

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
}

/// <summary>
/// 各コマンドの定義
/// </summary>
internal static class CommandDefine
{
    private static readonly Parser CustomParser = new(settings =>
    {
        settings.AutoHelp = false;
        settings.AutoVersion = false;
        settings.HelpWriter = null;
    });

    static CommandDefine()
    {
        SentenceBuilder.Factory = () => new LocalizableSentenceBuilder();
    }

    public static async Task RunAsync(SocketUserMessage message)
    {
        if (!message.Content.StartsWith(MasterManager.DiscordCommandPrefix)) return;

        var args = message.Content.Split(' ').Skip(1).ToArray();
        var parserResult = CustomParser.ParseArguments(args,
            typeof(UserOptions), typeof(MeOptions), typeof(RankingOptions), typeof(HelpOptions));

        var helpText = HelpText.AutoBuild(parserResult, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "";
                h.Copyright = "";
                h.AutoHelp = false;
                h.AutoVersion = false;
                return h;
            },
            e => e, false, 50);

        await parserResult.WithNotParsedAsync(async _ =>
        {
            var text = helpText.ToString();
            if (string.IsNullOrEmpty(text)) return;
            await message.ReplyAsync(
                embed: new EmbedBuilder().WithDescription($"```{text}```").Build());
        });

        await parserResult.MapResult(
            (UserOptions options) => DiscordManager.ExecuteAsync<UserPresenter>(message, async presenter =>
            {
                presenter.TargetUser = await message.Channel.GetUserAsync(options.User);
                presenter.IsTotal = options.Total;
            }),
            (MeOptions options) => DiscordManager.ExecuteAsync<UserPresenter>(message, presenter =>
            {
                presenter.IsTotal = options.Total;
                return Task.CompletedTask;
            }),
            (RankingOptions options) =>
                DiscordManager.ExecuteAsync<RankingPresenter>(message),
            (HelpOptions options) =>
                DiscordManager.ExecuteAsync<HelpPresenter>(message),
            errs => Task.CompletedTask);
    }

    [Verb("user", HelpText = "指定したユーザーの情報を表示する")]
    private class UserOptions
    {
        [Value(0, Required = true, HelpText = "ユーザーのDiscord ID", MetaName = "Discord ID")]
        public ulong User { get; set; }

        [Option("total", Default = false, HelpText = "指定した場合、累計の情報を表示する")]
        public bool Total { get; set; }
    }

    [Verb("me", HelpText = "自分の情報を表示する")]
    private class MeOptions
    {
        [Option("total", Default = false)] public bool Total { get; set; }
    }

    [Verb("ranking", HelpText = "ランキングを表示する")]
    private class RankingOptions
    {
    }

    [Verb("help", HelpText = "ヘルプを表示する")]
    private class HelpOptions
    {
    }
}

public class LocalizableSentenceBuilder : SentenceBuilder
{
    public override Func<string> RequiredWord => () => "【必須】";
    public override Func<string> OptionGroupWord => () => "【オプション】";
    public override Func<string> ErrorsHeadingText => () => "【エラー】";
    public override Func<string> UsageHeadingText => () => "【ヘルプ】";
    public override Func<bool, string> HelpCommandText => isOption => isOption ? "【オプション】" : "【ヘルプ】";
    public override Func<bool, string> VersionCommandText => _ => "【バージョン】";

    public override Func<Error, string> FormatError => error =>
    {
        switch (error.Tag)
        {
            case ErrorType.BadFormatTokenError:
                return $"'{((BadFormatTokenError)error).Token}' は認識できません。";
            case ErrorType.MissingValueOptionError:
                return $"引数 '{((MissingValueOptionError)error).NameInfo.NameText}' を指定してください。";
            case ErrorType.UnknownOptionError:
                return $"'{((UnknownOptionError)error).Token}' は認識できないオプションです。";
            case ErrorType.MissingRequiredOptionError:
                var errMissing = (MissingRequiredOptionError)error;
                if (errMissing.NameInfo.Equals(NameInfo.EmptyName)) return "必須のオプションが存在しません。";
                return $"オプション '{errMissing.NameInfo.NameText}' を指定してください。";
            case ErrorType.BadFormatConversionError:
                var badFormat = (BadFormatConversionError)error;
                if (badFormat.NameInfo.Equals(NameInfo.EmptyName)) return "オプションの形式が不正です。";
                return $"オプション '{badFormat.NameInfo.NameText}' の形式に誤りがないか確認してください。";
            case ErrorType.SequenceOutOfRangeError:
                var seqOutRange = (SequenceOutOfRangeError)error;
                if (seqOutRange.NameInfo.Equals(NameInfo.EmptyName)) return "オプションに指定した値の数が不正です。";
                return $"オプション '{seqOutRange.NameInfo.NameText}' に指定した値の数が不正です。";
            case ErrorType.BadVerbSelectedError:
                return $"コマンド '{((BadVerbSelectedError)error).Token}' は存在しません。";
            case ErrorType.NoVerbSelectedError:
                return "コマンドを指定してください。";
            case ErrorType.RepeatedOptionError:
                return $"オプション '{((RepeatedOptionError)error).NameInfo.NameText}' は1回のみ指定してください。";
            case ErrorType.SetValueExceptionError:
                var setValueError = (SetValueExceptionError)error;
                return $"'{setValueError.NameInfo.NameText}': {setValueError.Exception.Message}";
            case ErrorType.MissingGroupOptionError:
                var missingGroupOptionError = (MissingGroupOptionError)error;
                var options = string.Join(", ", missingGroupOptionError.Names.Select(n => n.NameText));
                return $"'{missingGroupOptionError.Group}' ({options}) の中から最低1つのオプションを指定してください。";
            case ErrorType.MultipleDefaultVerbsError:
                return MultipleDefaultVerbsError.ErrorMessage;
            default:
                return "不明なエラーです。";
        }
    };

    public override Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors => _ => "";
}