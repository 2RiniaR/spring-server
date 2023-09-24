using System.Text;
using Discord;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class PresenterBase
{
    public static class Format
    {
        public static string Bold(string text)
        {
            return $"**{text}**";
        }

        public static string CodeBlock(string text)
        {
            return $"```\n{text}\n```";
        }

        public static string Code(string text)
        {
            return $"`{text}`";
        }

        public static string Sanitize(string text)
        {
            foreach (var target in SanitizeTarget)
            {
                text = text.Replace(target, "\\" + target);
            }

            return text;
        }

        private static readonly string[] SanitizeTarget = { "\\", "*", "_", "~", "`", ".", ":", "/", ">", "|", "#" };

        public static string UserName(IUser user)
        {
            return Sanitize((user as SocketGuildUser)?.Nickname ?? user.Username);
        }

        public static string ScoreDiffGroup(int marvelous, int painful, bool onCode = false)
        {
            var elements = new List<string>();
            if (marvelous != 0) elements.Add(MarvelousScoreDiff(marvelous));
            if (painful != 0) elements.Add(PainfulScoreDiff(painful));
            var text = string.Join(" ", elements);
            return onCode ? text : Code(text);
        }

        public static string MarvelousScore(int diff, bool onCode = false)
        {
            var text = $"✨{diff}";
            return onCode ? text : Code(text);
        }

        public static string MarvelousScoreDiff(int diff, bool onCode = false)
        {
            var text = $"✨{diff:+#;-#;0}";
            return onCode ? text : Code(text);
        }

        public static string PainfulScore(int diff, bool onCode = false)
        {
            var text = $"💊{diff}";
            return onCode ? text : Code(text);
        }

        public static string PainfulScoreDiff(int diff, bool onCode = false)
        {
            var text = $"💊{diff:+#;-#;0}";
            return onCode ? text : Code(text);
        }

        public static string Time(TimeSpan time)
        {
            return $"{time.Hours:D2}:{time.Minutes:D2}";
        }

        public static string DateTime(DateTime dateTime)
        {
            return $"{dateTime:yyyy/MM/dd HH:mm}";
        }

        public static string MarvelousScoreIcon => "✨";
        public static string MarvelousScoreName => "えらいポイント";
        public static string PainfulScoreIcon => "💊";
        public static string PainfulScoreName => "よしよしポイント";

        public static string Table(IEnumerable<(string name, int count)> records)
        {
            var maxDigit = records.Max(r => r.Item2).ToString().Length;
            var sb = new StringBuilder();
            foreach (var (name, count) in records)
            {
                sb.AppendLine($"| {count.ToString().PadLeft(maxDigit)} | {Sanitize(name)}");
            }

            return sb.ToString();
        }
    }

    public async Task RunAsync()
    {
        try
        {
            await MainAsync();
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync("========================================\n");
            Console.Error.Write(e);
        }
    }

    protected abstract Task MainAsync();

    public static Embed ErrorEmbed(string message)
    {
        return new EmbedBuilder()
            .WithColor(Color.Red)
            .WithTitle("⚠ エラー")
            .WithDescription(message)
            .WithCurrentTimestamp()
            .Build();
    }
}