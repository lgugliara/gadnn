using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticTspSolver
{
    public class GeneticAlgorithm<T>
    {
        public Population<T> Population { get; set; }

        //public event EventHandler OnRan;
        //public event EventHandler OnTerminate;

        public Stopwatch Stopwatch { get; set; } = Stopwatch.StartNew();

        public GeneticAlgorithm(
            int chromosomes_count,
            int genes_count,
            List<T> values,
            Func<Chromosome<T>, double> evaluate,
            double comparer,
            double mutation_factor,
            bool isUnique
        ) {
            // Benchmark
            Stopwatch.Restart();

            Crossover<T>.Initialize();
            Mutation<T>.Initialize(Math.Max(1, (int)(Math.Min(1, mutation_factor) * genes_count)));
            Fitness<T>.Initialize(evaluate, comparer);
            Picker<T>.Initialize();

            if (isUnique)
            {
                System.Random rnd = new System.Random();
                var adam_pool = new List<T>(values);
                var adam_values = Enumerable
                    .Range(0, genes_count)
                    .Select(x => {
                        var index = adam_pool[rnd.Next(0, adam_pool.Count)];
                        adam_pool.Remove(index);
                        return index;
                    }).ToArray();
                Parallel.ForEach(
                    adam_values,
                    v => {
                        values.Remove(v);
                        values.Prepend(v);
                    }
                );
            }

            Population = new Population<T>(this, 0, chromosomes_count, genes_count, values.ToArray());

            UnityEngine.Debug.Log("First population created in " + Stopwatch.Elapsed);
        }

        public async Task Start()
        {
            Fitness<T>.Evaluate(Chromosome<T>.Adam);

            UnityEngine.Debug.LogAssertion("Adam fitness: " + Chromosome<T>.Adam.Fitness.Value);

            Run();
        }

        public void Run()
        {
            for(int generation = 0; !ITermination<T>.IsTerminated(this); generation++)
            {
                if(generation % 20 == 0)
                    UnityEngine.Debug.LogWarning("(GEN)\t" + generation + "(BEST FITNESS)\t" + Population.Best.Fitness.Value);

                // TODO
                //Population.PerformCrossover();
                Population.PerformMutate();
                Population.PerformEvaluate();
                Population.PerformPick();

                //OnRan?.Invoke(this, EventArgs.Empty);
            }

            //OnTerminate?.Invoke(this, EventArgs.Empty);
        }
    }
}
