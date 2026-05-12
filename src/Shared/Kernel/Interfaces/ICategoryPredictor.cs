namespace PDH.Shared.Kernel.Interfaces;

public interface ICategoryPredictor
{
    string PredictCategory(string title, string sourceProvider);
    void Train(IEnumerable<(string Title, string Provider, string Category)> trainingData);
}
