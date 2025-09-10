namespace MijnCopilot.Web.Extensions;

public static class DateTimeExtensions
{
    // From: https://rmauro.dev/calculate-time-ago-with-csharp/
    public static string AsTimeAgo(this DateTime dateTime)
    {
        TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTime);

        return timeSpan.TotalSeconds switch
        {
            <= 1 => "zojuist",
            <= 60 => $"{timeSpan.Seconds} seconden geleden",

            _ => timeSpan.TotalMinutes switch
            {
                <= 1 => "ongeveer een minuut geleden",
                < 60 => $"ongeveer {timeSpan.Minutes} minuten geleden",
                _ => timeSpan.TotalHours switch
                {
                    <= 1 => "ongeveer een uur geleden",
                    < 24 => $"ongeveer {timeSpan.Hours} uren geleden",
                    _ => timeSpan.TotalDays switch
                    {
                        <= 1 => "gisteren",
                        <= 30 => $"ongeveer {timeSpan.Days} dagen geleden",

                        <= 60 => "ongeveer een maand geleden",
                        < 365 => $"ongeveer {timeSpan.Days / 30} maanden geleden",

                        <= 365 * 2 => "ongeveer een jaar geleden",
                        _ => $"ongeveer {timeSpan.Days / 365} jaren geleden"
                    }
                }
            }
        };
    }
}