using System;

namespace GeneticTspSolver
{
    public class Fitness<T> : IFitness<T>
    {
        public double Value { get; set; } = 0;
        public void Evaluate(Chromosome<T> chromosome) {
            this.Value = IFitness<T>._Evaluate(chromosome);
        }

        public override string ToString()
        {
            return "Best fitness:\t\t" + this.Value.ToString("N0") + "\t\t\t" + (100 * this.Value / 19_913_031).ToString("F4");
        }
    }

    public interface IFitness<T>
    {
        public double Value { get; set; }

        public void Evaluate(Chromosome<T> chromosome);

        // Statics
        public static Func<Chromosome<T>, double> _Evaluate { get; set; }

        public static void From() {
            From((Chromosome<T> chromosome) => 0);
        }
        public static void From(Func<Chromosome<T>, double> evaluate) {
            _Evaluate = evaluate;
        }
    }
}
