using Microsoft.Extensions.Logging;

namespace PDH.ApplicationTests;

public class TestLogger<T> : ILogger<T>
{
    private readonly List<(LogLevel Level, EventId EventId, object? State)> _logEntries = new();

    public int CallCount => _logEntries.Count;

    public IReadOnlyList<(LogLevel Level, EventId EventId, object? State)> Entries => _logEntries;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _logEntries.Add((logLevel, eventId, state));
    }
}
