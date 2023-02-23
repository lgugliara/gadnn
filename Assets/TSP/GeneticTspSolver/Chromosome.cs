using System.Collections.Generic;
using System.Linq;

namespace GeneticTspSolver
{
    public class Chromosome<T> : IChromosome<T>
    {
        public int Id { get; set; }
        public List<T> Pool { get; set; }
        public List<Gene<T>> Genes { get; set; } = new List<Gene<T>>();
        public Fitness<T> Fitness { get; set; }
        public bool IsInitialized { get; set; }

        public ICrossover<T> Crossover { get; set; }

        public Chromosome(T[] values, List<T> pool, int id = 0) : this(pool, id)
        {
            foreach (var v in values)
                this.Genes.Add(new Gene<T>(this, v));
        }
        public Chromosome(List<T> pool, int id = 0)
        {
            this.Id = id;
            //this.Pool = Enumerable.Range(0, pool.Count).Select((p, index) => pool[index]).ToList();
            this.Pool = new List<T>(pool);
            this.Fitness = new Fitness<T>();
            this.IsInitialized = true;
            this.Crossover = new Crossover<T>();
        }

        public override string ToString()
        {
            return this.Genes.Select(g => g.ToString()).Aggregate((i, j) => i + ';' + j); 
        }
    }

    public interface IChromosome<T>
    {
        public int Id { get; set; }
        public List<T> Pool { get; set; }
        public List<Gene<T>> Genes { get; set; }
        public Fitness<T> Fitness { get; set; }
        public bool IsInitialized { get; set; }

        public ICrossover<T> Crossover { get; set; }

        public static Chromosome<T> From(Chromosome<T> from, int id)
        {
            var to = new Chromosome<T>(from.Pool, id);
            to.Genes = from.Genes.Select(g => IGene<T>.From(g, to)).ToList();

            return to;
        }
    }
}
