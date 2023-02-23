using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GeneticTspSolver
{
    public class GeneticAlgorithm<T> : IGeneticAlgorithm<T>
    {
        public IPopulation<T> Population { get; set; }
        public ITermination<T> Termination { get; set; } = new EndlessTermination<T>();
        public int GenerationNumber { get; set; }

        // Event handlers
        public event EventHandler OnRan;
        public event EventHandler OnTerminate;

        // Debug
        public Stopwatch Stopwatch { get; set; } = Stopwatch.StartNew();

        public GeneticAlgorithm(int size, T[] values, Func<Chromosome<T>, double> evaluate, T[] pool)
        {
            IFitness<T>.From(evaluate);
            this.Population = new Population<T>(size, values, pool);
            UnityEngine.Debug.Log("First population created in " + Stopwatch.Elapsed);
        }

        public async Task Start()
        {
            Population.Adam.Fitness.Evaluate(Population.Adam);
            UnityEngine.Debug.Log("Adam fitness: " + Population.Adam.Fitness.Value);

            Run();
        }

        public void Run()
        {
            for (this.GenerationNumber = 0; !Termination.IsTerminated(this); this.GenerationNumber++)
            {
                //if(this.GenerationNumber % 20 == 0)
                //    UnityEngine.Debug.Log("Generation:\t" + this.GenerationNumber + "\tScore:\t\t" + this.Population.Best.Fitness.Value);

                //Population.PerformCrossover();
                Population.PerformMutate();
                Population.PerformEvaluate();

                OnRan?.Invoke(this, EventArgs.Empty);
            }

            OnTerminate?.Invoke(this, EventArgs.Empty);
        }
    }

    public interface IGeneticAlgorithm<T>
    {
        public IPopulation<T> Population { get; set; }
        public ITermination<T> Termination { get; set; }
        public int GenerationNumber { get; set; }

        // Event handlers
        public event EventHandler OnRan;
        public event EventHandler OnTerminate;

        // Debug
        public Stopwatch Stopwatch { get; set; }

        public void Run();
    }
}
