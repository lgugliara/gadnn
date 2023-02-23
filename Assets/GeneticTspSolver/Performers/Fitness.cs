using System;

namespace GeneticTspSolver
{
    public class Fitness<T>
    {
        public double Value { get; private set; } = 0;

        public static double Comparer { get; private set; } = 1;

        public double Percent => (100 * Value / Comparer);

        private static Func<Chromosome<T>, double> _evaluate { get; set; } = (Chromosome<T> chromosome) => 0;

        public static void Evaluate(Chromosome<T> chromosome) {
            chromosome.Fitness.Value = Fitness<T>._evaluate(chromosome);
        }

        public override string ToString()
        {
            return "(fitness)\t" + Value.ToString("N0") + "\t(% over comparer value)" + Percent.ToString("F4");
        }

        public static void Initialize(Func<Chromosome<T>, double> evaluate, double comparer)
        {
            Fitness<T>._evaluate = evaluate;

            if (comparer > 0)
                Comparer = comparer;
            else throw new ArgumentException("Comparer must be greater than 0");
        }
    }
}
