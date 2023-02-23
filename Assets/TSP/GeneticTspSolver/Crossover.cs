using System.Collections.Generic;

namespace GeneticTspSolver
{
    public class Crossover<T> : ICrossover<T>
    {
        public IEnumerable<IChromosome<T>> Cross(IChromosome<T> chromosome, IChromosome<T> other)
        {
            return new[] { chromosome, other };
        }
    }

    public interface ICrossover<T>
    {
        public IEnumerable<IChromosome<T>> Cross(IChromosome<T> chromosome, IChromosome<T> other);
    }
}
