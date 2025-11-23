using Application.Services.Analyses;

namespace Application.Services.CalculatorFactory;

public class AnalysisClusteringFactory : AnalysisCalculatorFactory<List<List<StudentPerformanceDto>>>
{
    private readonly IAnalysisCalculatorFactory<AdvancedStatisticsData> _innerCalculator;

    public AnalysisClusteringFactory(IAnalysisCalculatorFactory<AdvancedStatisticsData> innerCalculator,IAnalysisService analysisService) : base(analysisService)
    {
        _innerCalculator = innerCalculator;
    }
    
    public override async Task<List<List<StudentPerformanceDto>>> CalculateAsync(int analysisId, CancellationToken cancellationToken = default)
    {
        AdvancedStatisticsData advancedStatisticsData =  await _innerCalculator.CalculateAsync(analysisId, cancellationToken);

        List<StudentPerformanceDto> students = advancedStatisticsData.ClusteringData;

        return PerformKMeansClustering(students, 4);
    }

    private List<List<StudentPerformanceDto>> PerformKMeansClustering(List<StudentPerformanceDto> students, int clusterCount)
    {
        if (students.Count < clusterCount)
            clusterCount = students.Count;

        var normalizedData = NormalizeData(students);

        var clusters = KMeans(normalizedData, clusterCount);

        var result = new List<List<StudentPerformanceDto>>();
        for (int i = 0; i < clusterCount; i++)
        {
            result.Add(new List<StudentPerformanceDto>());
        }

        for (int i = 0; i < students.Count; i++)
        {
            result[clusters[i]].Add(students[i]);
        }

        return result;
    }

    private List<double[]> NormalizeData(List<StudentPerformanceDto> students)
    {
        var normalized = new List<double[]>();

        var scores = students.Select(s => s.TotalScore).ToList();
        var blankRates = students.Select(s => s.BlankRate).ToList();

        double scoreMin = scores.Min();
        double scoreMax = scores.Max();
        double blankMin = blankRates.Min();
        double blankMax = blankRates.Max();

        foreach (var student in students)
        {
            normalized.Add(new[]
            {
                Normalize(student.TotalScore, scoreMin, scoreMax),
                Normalize(student.BlankRate, blankMin, blankMax)
            });
        }

        return normalized;
    }

    private double Normalize(double value, double min, double max)
    {
        if (max == min) return 0;
        return (value - min) / (max - min);
    }

    private int[] KMeans(List<double[]> data, int k, int maxIterations = 100)
    {
        var random = new Random(42);
        int n = data.Count;
        int dim = data[0].Length;

        var centroids = new List<double[]>();
        var usedIndices = new HashSet<int>();
        for (int i = 0; i < k; i++)
        {
            int idx;
            do
            {
                idx = random.Next(n);
            } while (usedIndices.Contains(idx));

            usedIndices.Add(idx);
            centroids.Add((double[])data[idx].Clone());
        }

        int[] assignments = new int[n];

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            bool changed = false;

            for (int i = 0; i < n; i++)
            {
                int nearest = FindNearestCentroid(data[i], centroids);
                if (assignments[i] != nearest)
                {
                    assignments[i] = nearest;
                    changed = true;
                }
            }

            if (!changed) break;

            for (int c = 0; c < k; c++)
            {
                var clusterPoints = data.Where((_, idx) => assignments[idx] == c).ToList();
                if (clusterPoints.Any())
                {
                    for (int d = 0; d < dim; d++)
                    {
                        centroids[c][d] = clusterPoints.Average(p => p[d]);
                    }
                }
            }
        }

        return assignments;
    }

    private int FindNearestCentroid(double[] point, List<double[]> centroids)
    {
        int nearest = 0;
        double minDist = double.MaxValue;

        for (int i = 0; i < centroids.Count; i++)
        {
            double dist = EuclideanDistance(point, centroids[i]);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = i;
            }
        }

        return nearest;
    }

    private double EuclideanDistance(double[] a, double[] b)
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i++)
        {
            double diff = a[i] - b[i];
            sum += diff * diff;
        }
        return Math.Sqrt(sum);
    }
}
