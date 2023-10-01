// from https://ericlippert.com/2019/02/07/fixing-random-part-3/

public interface IDistribution<T>
{
    T Sample();
}

using SCU = StandardContinuousUniform;
public sealed class StandardContinuousUniform : IDistribution<double>
{
    public static readonly StandardContinuousUniform Distribution = new StandardContinuousUniform();
    private StandardContinuousUniform() { }
    public double Sample() => Pseudorandom.NextDouble();
}

using static System.Math;
using SCU = StandardContinuousUniform;
public sealed class Normal : IDistribution<double>
{
    public double Mean { get; }
    public double Sigma { get; }
    public double mean => Mean;
    public double sigma => Sigma;
    public readonly static Normal Standard = Distribution(0, 1);
    public static Normal Distribution(double mean, double sigma) => new Normal(mean, sigma);
    private Normal(double mean, double sigma)
    {
        this.Mean = mean;
        this.Sigma = sigma;
    }

    // Box-Muller method
    private double StandardSample() =>
      Sqrt(-2.0 * Log(SCU.Distribution.Sample())) *
        Cos(2.0 * PI * SCU.Distribution.Sample());
    public double Sample() => mean + sigma * StandardSample();
}

public static class Distribution
{
    public static IEnumerable<T> Samples<T>(this IDistribution<T> d)
    {
        while (true)
            yield return d.Sample();
    }

    public static string Histogram(this IDistribution<double> d, double low, double high) => d.Samples().Histogram(low, high);

    public static string Histogram(this IEnumerable<double> d, double low, double high)
    {
        const int width = 40;
        const int height = 20;
        const int sampleCount = 100000;
        int[] buckets = new int[width];
        foreach (double c in d.Take(sampleCount))
        {
            int bucket = (int)(buckets.Length * (c - low) / (high - low));
            if (0 <= bucket && bucket < buckets.Length)
                buckets[bucket] += 1;
        }

        int max = buckets.Max();
        double scale = max < height ? 1.0 : ((double)height) / max;
        return string.Join("",
          Enumerable.Range(0, height).Select(
            r => string.Join("", buckets.Select(
              b => b * scale > (height - r) ? '*' : ' ')
            ) + "\n"
          )
        ) + new string('-', width) + "\n";
    }
}

// SCU.Distribution.Samples().Take(12).Sum()
