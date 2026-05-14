namespace Application.Helpers;

public static class TimeZoneHelper
{
    public static TimeZoneInfo SaoPaulo()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
    }

    public static DateTime? ToSaoPaulo(DateTime? utcDate)
    {
        if (utcDate == null) return null;

        var utc = DateTime.SpecifyKind( utcDate.Value, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utc, SaoPaulo());
    }
}
