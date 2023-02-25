using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace GeneticTspSolver
{
    public static class Picker<T>
    {
        public static void Pick(Population<T> population)
        {
            var best_of_generation = population.Chromosomes.Max();

            if (best_of_generation.Fitness.Value > population.Best.Fitness.Value)
            {
                population.Best = best_of_generation;

                Parallel.ForEach(
                    population.Chromosomes,
                    c => Chromosome<T>.Copy(population.Best, c)
                );
                UnityEngine.Debug.Log(population.Best.Fitness.ToString());
            }
        }

        public static void Initialize()
        {

        }
    }
}