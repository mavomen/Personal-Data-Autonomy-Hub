using PDH.Modules.ML;
using Xunit;

namespace PDH.ApplicationTests.ML;

public class CategoryPredictorTests
{
    [Fact]
    public void PredictCategory_Untrained_Returns_DeepWork()
    {
        var predictor = new CategoryPredictor();
        var result = predictor.PredictCategory("coding", "GitHub");
        Assert.Equal("DeepWork", result);
    }

    [Fact]
    public void Train_And_Predict_Works()
    {
        var predictor = new CategoryPredictor();
        var training = new[]
        {
            ("Title1", "GitHub", "DeepWork"),
            ("Title2", "Calendar", "Social"),
            ("Title3", "Health app", "Health"),
            ("Title4", "Email", "Admin"),
        };
        predictor.Train(training);
        var prediction = predictor.PredictCategory("Team meeting", "Calendar");
        Assert.True(prediction is "Social" or "Admin");
    }
}
