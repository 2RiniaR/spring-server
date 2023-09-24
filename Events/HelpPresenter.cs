using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class HelpPresenter : DiscordMessagePresenterBase
{
    protected override async Task MainAsync()
    {
        var embed = new EmbedBuilder()
            .WithColor(Color.Blue)
            .WithTitle("エライさん")
            .WithDescription(Text)
            .WithCurrentTimestamp()
            .Build();

        // DMでヘルプを送る
        await Message.Author.SendMessageAsync(embed: embed);
    }

    public static string Text
        => $"""
            あなたのえらさを表す「{Format.MarvelousScoreIcon}{Format.MarvelousScoreName}」、つらさを表す「{Format.PainfulScoreIcon}{Format.PainfulScoreName}」を集めるBOTです。

            ### 👏 褒める {Format.MarvelousScoreDiff(MasterManager.SendPraiseMarvelousScore)} （相手に{Format.MarvelousScoreDiff(MasterManager.ReceivePraiseMarvelousScore)}）
            他人のメッセージにリアクション（{string.Join("", MasterManager.PraiseEmotes)}）を付ける

            ### 😥 慰める {Format.MarvelousScoreDiff(MasterManager.SendComfortMarvelousScore)} （相手に{Format.PainfulScoreDiff(MasterManager.ReceiveComfortPainfulScore)}）
            他人のメッセージにリアクション（{string.Join("", MasterManager.ComfortEmotes)}）を付ける

            ### 😎 生きる {Format.MarvelousScoreDiff(MasterManager.LoginMarvelousScore)}
            その日最初のメッセージを送る

            ### 💤健康な睡眠を取る {Format.MarvelousScoreDiff(MasterManager.WakeUpMarvelousScore)} （失敗時は {Format.PainfulScoreDiff(MasterManager.WakeUpFailedPainfulScore)}）
            `{Format.Time(MasterManager.BedInStart)} ~ {Format.Time(MasterManager.BedInEnd)}`に「{MasterManager.BedInPhrase}」と送り、次のメッセージを`{Format.Time(MasterManager.WakeUpStart)} ~ {Format.Time(MasterManager.WakeUpEnd)}`に送る

            ### コマンド
             - {Format.Code("!erai user [ユーザーID]")} : 指定したユーザーのステータスを表示する
             - {Format.Code("!erai me")}      : 自分のステータスを表示する
             - {Format.Code("!erai ranking")} : ランキングを表示する
             - {Format.Code("!erai help")}    : ヘルプを表示する
                     
            ### 開発: Rinia（@2RIniaR）
            問題点・改善案は、[こちら](https://github.com/2RiniaR/spring-server/issues)から報告をお願いします。
            """;
}