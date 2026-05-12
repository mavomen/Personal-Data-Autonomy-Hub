using Microsoft.ML.Data;

namespace PDH.Modules.ML;

public class ActivityData
{
    [LoadColumn(0)]
    public string Title { get; set; } = string.Empty;

    [LoadColumn(1)]
    public string SourceProvider { get; set; } = string.Empty;

    [LoadColumn(2)]
    public string Category { get; set; } = string.Empty;
}
