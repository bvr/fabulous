/* from 
    - https://ericlippert.com/2019/02/07/fixing-random-part-3/
    - https://ericlippert.com/2019/02/11/fixing-random-part-4/
    - https://ericlippert.com/2019/02/14/fixing-random-part-5/

*/

using static System.Math;

﻿using System;
using System.Threading;

namespace Probability
{
    // A threadsafe, all-static, crypto-randomized wrapper around Random.
    // Still not great, but a slight improvement.    
    public static class Pseudorandom
    {
        private readonly static ThreadLocal<Random> prng =
          new ThreadLocal<Random>(() => new Random(BetterRandom.NextInt()));
        public static int NextInt() => prng.Value.Next();
        public static double NextDouble() => prng.Value.NextDouble();
    }    

    public interface IDistribution<T>
    {
        T Sample();
    }

    public interface IDiscreteDistribution<T> : IDistribution<T>
    {
        IEnumerable<T> Support();
        int Weight(T t);
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

        public static string Histogram<T>(this IDiscreteDistribution<T> d) => d.Samples().DiscreteHistogram();

        public static string DiscreteHistogram<T>(this IEnumerable<T> d)
        {
            const int sampleCount = 100000;
            const int width = 40;
            var dict = d.Take(sampleCount)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());
            int labelMax = dict.Keys
                .Select(x => x.ToString().Length)
                .Max();
            var sup = dict.Keys.OrderBy(x => x).ToList();
            int max = dict.Values.Max();
            double scale = max < width ? 1.0 : ((double)width) / max;
            return string.Join("\n", sup.Select(s => $"{ToLabel(s)}|{Bar(s)}"));

            string ToLabel(T t) => t.ToString().PadLeft(labelMax);
            string Bar(T t) => new string('*', (int)(dict[t] * scale));
        }

        public static string ShowWeights<T>(this IDiscreteDistribution<T> d)
        {
            int labelMax = d.Support()
                .Select(x => x.ToString().Length)
                .Max();
            return string.Join("\n", d.Support().Select(s => $"{ToLabel(s)}:{d.Weight(s)}"));
            string ToLabel(T t) => t.ToString().PadLeft(labelMax);
        }
    }

    public sealed class StandardContinuousUniform : IDistribution<double>
    {
        public static readonly StandardContinuousUniform Distribution = new StandardContinuousUniform();
        private StandardContinuousUniform() { }
        public double Sample() => Pseudorandom.NextDouble();
    }

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


    public sealed class StandardDiscreteUniform : IDiscreteDistribution<int>
    {
        public static StandardDiscreteUniform Distribution(int min, int max)
        {
            if (min > max)
                throw new ArgumentException();
            return new StandardDiscreteUniform(min, max);
        }

        public int Min { get; }
        public int Max { get; }

        private StandardDiscreteUniform(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public IEnumerable<int> Support() => Enumerable.Range(Min, 1 + Max - Min);
        public int Sample() => (int)((SCU.Distribution.Sample() * (1.0 + Max - Min)) + Min);
        public int Weight(int i) => (Min <= i && i <= Max) ? 1 : 0;
        public override string ToString() => $"StandardDiscreteUniform[{Min}, {Max}]";
    }

    using SDU = StandardDiscreteUniform;

}

/* Tests 

SCU.Distribution.Histogram(0, 1);
SCU.Distribution.Samples().Take(12).Sum();
Normal.Distribution(1.0, 1.5).Histogram(–4, 4);
SDU.Distribution(1, 6).Samples().Take(10).Sum();
SDU.Distribution(1, 10).Histogram();

*/
