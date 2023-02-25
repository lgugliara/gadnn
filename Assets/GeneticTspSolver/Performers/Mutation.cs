using System;
using System.Linq;

namespace GeneticTspSolver
{
    public static class Mutation<T>
    {
        public static int MaxSwaps = 1;

        public static void Mutate(Chromosome<T> chromosome)
        {
            var random = new Random();
            var swap_count = random.Next(1, MaxSwaps);

            for (int i = 0; i < swap_count; i++)
            {
                var new_value = chromosome.Parent.AllValues[random.Next(0, chromosome.Parent.AllValues.Length)];
                var to_gene = chromosome.Genes[random.Next(0, chromosome.Genes.Count)];
                int same_gene_id;

                if (chromosome.Lookup.TryGetValue(new_value, out same_gene_id))
                    chromosome.Genes[same_gene_id].Value = to_gene.Value;

                to_gene.Value = new_value;
            }
        }

        public static void Initialize(int max_swaps)
        {
            if (max_swaps > 0)
                MaxSwaps = max_swaps;
            else throw new ArgumentException("Max swaps must be greater than 0");
        }
    }
}
