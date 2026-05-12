using Microsoft.ML;
using PDH.Shared.Kernel.Interfaces;

namespace PDH.Modules.ML;

public class CategoryPredictor : ICategoryPredictor
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private bool _isTrained;

    public CategoryPredictor()
    {
        _mlContext = new MLContext(seed: 42);
        _model = null!;
    }

    public string PredictCategory(string title, string sourceProvider)
    {
        if (!_isTrained)
            return "DeepWork";

        var engine = _mlContext.Model.CreatePredictionEngine<ActivityData, ActivityPrediction>(_model);
        var prediction = engine.Predict(new ActivityData
        {
            Title = title,
            SourceProvider = sourceProvider
        });
        return prediction.Category;
    }

    public void Train(IEnumerable<(string Title, string Provider, string Category)> trainingData)
    {
        var mlData = trainingData.Select(t => new ActivityData
        {
            Title = t.Title,
            SourceProvider = t.Provider,
            Category = t.Category
        });

        var dataView = _mlContext.Data.LoadFromEnumerable(mlData);

        var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(ActivityData.Category))
            .Append(_mlContext.Transforms.Text.FeaturizeText("TitleFeat", nameof(ActivityData.Title)))
            .Append(_mlContext.Transforms.Text.FeaturizeText("ProviderFeat", nameof(ActivityData.SourceProvider)))
            .Append(_mlContext.Transforms.Concatenate("Features", "TitleFeat", "ProviderFeat"))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        _model = pipeline.Fit(dataView);
        _isTrained = true;
    }
}
