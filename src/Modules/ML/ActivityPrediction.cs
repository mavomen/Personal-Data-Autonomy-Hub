using Microsoft.ML.Data;

namespace PDH.Modules.ML;

public class ActivityPrediction
{
    [ColumnName("PredictedLabel")]
    public string Category { get; set; } = string.Empty;
    public float[]? Scores { get; set; }
}
