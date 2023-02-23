using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticTspSolver
{
    public class Population<T> : IPopulation<T>
    {
        public int Size { get; set; }
        public Chromosome<T> Adam { get; private set; }
        public List<Chromosome<T>> Chromosomes { get; set; }
        public Chromosome<T> Best
        {
            get
            {
                return this.Chromosomes[this._BestId];
            }
        }

        private int _BestId = 0;

        // TODO
        public Gene<T>[] allGenes;

        public Stopwatch Stopwatch { get; set; } = Stopwatch.StartNew();

        public Population(int size, T[] values, T[] pool)
        {
            var adam = new Chromosome<T>(values, pool.ToList());
            this.Size = size;
            this.Adam = adam;
            this.Chromosomes = Enumerable.Range(0, size).Select(index => IChromosome<T>.From(adam, index)).ToList();
        }

        public void PerformCrossover()
        {
            // this.Stopwatch.Restart();
            var creators = this.Chromosomes.OrderByDescending(x => x.Fitness.Value).Take(2).ToList();

            Parallel.ForEach(
                this.Chromosomes.Skip(2),
                c => {
                    // TODO
                    c.Crossover.Cross(creators[0], creators[1]);
                }
            );
            // UnityEngine.Debug.Log("Crossover done in " + Stopwatch.Elapsed);
        }
        public void PerformMutate()
        {
            // this.Stopwatch.Restart();
            Parallel.ForEach(
                //this.Chromosomes.Skip(2),
                this.Chromosomes.AsParallel().Where(c => c.Id != this._BestId),
                c => ChromosomeMutation<T>.Mutate(c)
            );
            // UnityEngine.Debug.Log("Mutation done in " + Stopwatch.Elapsed);
        }
        public void PerformEvaluate()
        {
            // this.Stopwatch.Restart();
            Parallel.ForEach(
                this.Chromosomes,
                c => c.Fitness.Evaluate(c)
            );
            // UnityEngine.Debug.Log("Evaluation done in " + Stopwatch.Elapsed);
        }
        public void PerformPick()
        {
            // this.Stopwatch.Restart();
            var maxFitness = this.Chromosomes.Max(x => x.Fitness.Value);
            if (maxFitness > this.Best.Fitness.Value)
            {
                this._BestId = this.Chromosomes.FirstOrDefault(x => x.Fitness.Value == maxFitness).Id;
                UnityEngine.Debug.Log(this.Best.Fitness.ToString());
            }
            // UnityEngine.Debug.Log("Picking done in " + Stopwatch.Elapsed);
        }
    }

    public interface IPopulation<T>
    {
        public int Size { get; set; }
        public Chromosome<T> Adam { get; }
        public List<Chromosome<T>> Chromosomes { get; set; }
        public Chromosome<T> Best { get; }

        public void PerformCrossover();
        public void PerformMutate();
        public void PerformEvaluate();
        public void PerformPick();
    }
}
