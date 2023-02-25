using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace GeneticTspSolver
{
    public class Chromosome<T> : IComparable
    {
        public Population<T> Parent { get; set; }
        public int Id { get; private set; }

        public Fitness<T> Fitness { get; set; } = new Fitness<T>();

        public ArraySegment<T> Values;
        public ArraySegment<Gene<T>> Genes;
        public Dictionary<T, int> Lookup;

        public int GenesCount => Values.Count;

        public static Chromosome<T> Adam { get; set; }

        public Chromosome(Population<T> parent, int id, int genes_count)
        {
            Parent = parent;
            Id = id;
            Fitness = new Fitness<T>();

            Values = new ArraySegment<T>(parent.AllValues, genes_count * id, genes_count);
            Genes = new ArraySegment<Gene<T>>(parent.AllGenes, genes_count * id, genes_count);

            Chromosome<T>._TransferTo(this, this);
        }

        private static void _TransferTo(Chromosome<T> chromosome, Chromosome<T> to)
        {
            to.Lookup = to.Genes.AsParallel().Select((g, i) =>
            {
                if (g == null)
                    to.Genes[(int)i] = new Gene<T>(chromosome, (int)i, chromosome.Values[(int)i]);
                else
                    to.Genes[(int)i].Value = chromosome.Values[(int)i];

                return new { v = to.Values[i], i };
            }).ToDictionary(x => x.v, x => x.i);
        }

        public override string ToString() => Genes.Select(g => g.ToString()).Aggregate((i, j) => i + ';' + j);

        public static Chromosome<T> From(Chromosome<T> from, int id) => new Chromosome<T>(from.Parent, id, from.GenesCount);

        public static void Copy(Chromosome<T> from, Chromosome<T> to) => Chromosome<T>._TransferTo(from, to);

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            Chromosome<T> other = obj as Chromosome<T>;
            if (other != null)
                return Fitness.Value.CompareTo(other.Fitness.Value);
            else
                throw new ArgumentException("Object is not a Chromosome");
        }
    }
}
