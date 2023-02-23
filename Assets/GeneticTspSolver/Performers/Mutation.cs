using System;
using System.Linq;

namespace GeneticTspSolver
{
    public static class Mutation<T>
    {
        //public static int InsertMutationNumber = 50;
        //public static int RemoveMutationNumber = 50;
        public static int MaxSwaps = 1;

        public static void Mutate(Chromosome<T> chromosome)
        {
            //for (int i = 0; i < random.Next(0, InsertMutationNumber); i++)
            //{
            //    int fromindex = random.Next(0, gene.Pool.Count);
            //    int toindex = random.Next(0, random.Next(0, chromosome.Genes.Count()));
            //    var insertgene = chromosome.Genes.Last();
            //    gene.Pool.Append(insertgene.Value);
            //    insertgene.Value = gene.Pool[fromindex];
            //    gene.Pool.Remove(insertgene.Value);
            //    chromosome.Genes.Insert(toindex, insertgene);
            //    chromosome.Genes.RemoveAt(chromosome.Genes.Count() - 1);
            //}

            //for (int i = 0; i < random.Next(0, RemoveMutationNumber); i++)
            //{
            //    int fromindex = random.Next(0, random.Next(chromosome.Genes.Count()));
            //    var removegene = chromosome.Genes[fromindex];
            //    gene.Pool.Add(removegene.Value);
            //    removegene.Value = gene.Pool[0];
            //    gene.Pool.Remove(removegene.Value);
            //    chromosome.Genes.RemoveAt(fromindex);
            //    chromosome.Genes.Add(removegene);
            //}

            var random = new Random();
            var swap_count = random.Next(1, MaxSwaps);

            for (int i = 0; i < swap_count; i++)
            {
                int index_from = random.Next(0, chromosome.Genes.Count());
                int index_to = random.Next(0, chromosome.Genes.Count());

                var gene_from = chromosome.Genes[index_from];
                var gene_to = chromosome.Genes[index_to];

                (gene_to.Value, gene_from.Value) = (gene_from.Value, gene_to.Value);
            }
        }

        public static void Initialize(int max_swaps)
        {
            if (max_swaps > 0)
                MaxSwaps = max_swaps;
            else throw new ArgumentException("Max swaps must be greater than 0");
        }
    }

    //public static class GeneMutation<T>
    //{
    //    private static Random random = new Random();

    //    public static void Mutate(Gene<T> gene)
    //    {
    //        var converted = gene;
    //        converted.Pool.Add(converted.Value);
    //        var newValue = converted.Pool[random.Next(converted.Pool.Count)];
    //        converted.Pool.Remove(newValue);
    //        converted.Value = newValue;
    //    }
    //}
}
