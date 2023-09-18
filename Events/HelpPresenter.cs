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

    public string Text
        => $"""
            あなたのえらさを表す「{MarvelousScoreIcon}{MarvelousScoreName}」、つらさを表す「{PainfulScoreIcon}{PainfulScoreName}」を集めるBOTです。

            ### 👏 褒める {MarvelousScoreDiffText(MasterManager.SendPraiseMarvelousScore)} （相手に{MarvelousScoreDiffText(MasterManager.ReceivePraiseMarvelousScore)}）

            他人のメッセージにリアクション（{string.Join("", MasterManager.PraiseEmotes)}）を付ける

            ### 😥 慰める {MarvelousScoreDiffText(MasterManager.SendComfortMarvelousScore)} （相手に{PainfulScoreDiffText(MasterManager.ReceiveComfortPainfulScore)}）

            他人のメッセージにリアクション（{string.Join("", MasterManager.ComfortEmotes)}）を付ける

            ### 😎 生きる {MarvelousScoreDiffText(MasterManager.LoginMarvelousScore)}

            その日最初のメッセージを送る

            ### 💦 頑張る {MarvelousScoreDiffText(MasterManager.DailyContributionMarvelousScore)}

            GitHubにContributionを1日1回以上する ※GitHub IDの登録が必要

            ### 💤健康な睡眠を取る {MarvelousScoreDiffText(MasterManager.WakeUpMarvelousScore)} （失敗時は {PainfulScoreDiffText(MasterManager.WakeUpFailedPainfulScore)}）

            `{TimeText(MasterManager.BedInStart)} ~ {TimeText(MasterManager.BedInEnd)}`に「{MasterManager.BedInPhrase}」と送り、次のメッセージを`{TimeText(MasterManager.WakeUpStart)} ~ {TimeText(MasterManager.WakeUpEnd)}`に送る

            ### コマンド
             
             - `!erai me`      : 自分のステータスを表示する
             - `!erai ranking` : ランキングを表示する
             - `!erai help`    : ヘルプを表示する
             - `!erai github register [GitHub ID]` : GitHub IDを登録する
             - `!erai github unregister`           : GitHub IDの登録を解除する
                     
            ### 開発: Rinia（@2RIniaR）
                     
            問題点・改善案は、[こちら](https://github.com/2RiniaR/spring-server/issues)から報告をお願いします。
            """;
}