using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Diagnostics;

namespace Infra.Data.Interseptors;

public class SlowQueryInterceptor(ILogger<SlowQueryInterceptor> logger) : DbCommandInterceptor
{
    private readonly ILogger<SlowQueryInterceptor> _logger = logger;
    private readonly long _thresholdMs = 50;

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var response = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        
        stopwatch.Stop();
        
        if (stopwatch.ElapsedMilliseconds > _thresholdMs)
        {
            _logger.LogWarning(
                "Slow query detected ({Elapsed} ms): {CommandText}",
                stopwatch.ElapsedMilliseconds,
                command.CommandText);
        }

        return response;
    }
}
