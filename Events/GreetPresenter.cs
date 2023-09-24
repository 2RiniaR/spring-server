using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class GreetPresenter : DiscordMessagePresenterBase
{
    protected override async Task MainAsync()
    {
        var wakeUp = await UserServices.As(Message.Author.Id).WakeUpAsync();
        if (wakeUp == null) return;

        var name = Format.Sanitize(Message.Author.Username);
        var score = Format.ScoreDiffGroup(wakeUp.MarvelousScore, wakeUp.PainfulScore);

        switch (wakeUp.ResultType)
        {
            case WakeUpResultType.Succeed:
                await Message.ReplyAsync($"早寝早起きは健康の証！今日はきっといいことあるよ、{name}！ {score}");
                break;
            case WakeUpResultType.TooEarly:
                await Message.ReplyAsync($"まだ起きてたんだね。こんな時間までお疲れさま、{name}。 {score}");
                break;
            case WakeUpResultType.TooLate:
                await Message.ReplyAsync($"時間は遅くても、{name}の一日はこれからだよ！頑張ろうね！ {score}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}