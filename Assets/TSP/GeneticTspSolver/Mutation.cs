using System;
using System.Linq;

namespace GeneticTspSolver
{
    public static class ChromosomeMutation<T>
    {
        private static Random random = new Random();

        public static int InsertMutationNumber = 50;
        public static int RemoveMutationNumber = 50;
        public static int SwapNumber = 500;

        public static void Mutate(Chromosome<T> chromosome)
        {
            var gene = chromosome.Genes[0];

            for (int i = 0; i < random.Next(0, InsertMutationNumber); i++)
            {
                int fromindex = random.Next(0, gene.Pool.Count);
                int toindex = random.Next(0, random.Next(0, chromosome.Genes.Count()));

                var insertgene = chromosome.Genes.Last();
                gene.Pool.Append(insertgene.Value);
                insertgene.Value = gene.Pool[fromindex];
                gene.Pool.Remove(insertgene.Value);
                chromosome.Genes.Insert(toindex, insertgene);
                chromosome.Genes.RemoveAt(chromosome.Genes.Count() - 1);
            }

            for (int i = 0; i < random.Next(0, RemoveMutationNumber); i++)
            {
                int fromindex = random.Next(0, random.Next(chromosome.Genes.Count()));

                var removegene = chromosome.Genes[fromindex];
                gene.Pool.Add(removegene.Value);
                removegene.Value = gene.Pool[0];
                gene.Pool.Remove(removegene.Value);
                chromosome.Genes.RemoveAt(fromindex);
                chromosome.Genes.Add(removegene);
            }

            for (int i = 0; i < random.Next(0, SwapNumber); i++)
            {
                int swapindex = random.Next(0, chromosome.Genes.Count());
                int swapindex2 = random.Next(0, chromosome.Genes.Count());
                (
                    (chromosome.Genes[swapindex2]).Value,
                    (chromosome.Genes[swapindex]).Value
                ) = (
                    (chromosome.Genes[swapindex]).Value,
                    (chromosome.Genes[swapindex2]).Value
                );
            }
        }
    }

    public static class GeneMutation<T>
    {
        private static Random random = new Random();

        public static void Mutate(Gene<T> gene)
        {
            var converted = gene;
            converted.Pool.Add(converted.Value);
            var newValue = converted.Pool[random.Next(converted.Pool.Count)];
            converted.Pool.Remove(newValue);
            converted.Value = newValue;
        }
    }
}
