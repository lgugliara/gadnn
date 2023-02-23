using System;
using System.Collections.Generic;

namespace GeneticTspSolver
{
    public class Gene<T> : IGene<T>
    {
        public Chromosome<T> Parent { get; set; }
        public List<T> Pool { get
            {
                return this.Parent.Pool;
            }
        }
        public T Value { get; set; }
        public bool IsInitialized { get; set; }

        public ICrossover<T> Crossover { get; set; }

        public Gene(Chromosome<T> parent, T value)
        {
            this.Parent = parent;
            this.Value = value;
            this.IsInitialized = true;

            this.Crossover = new Crossover<T>();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }

    public interface IGene<T>
    {
        public List<T> Pool { get; }
        public T Value { get; set; }
        public bool IsInitialized { get; set; }
        public Chromosome<T> Parent { get; set; }

        public ICrossover<T> Crossover { get; set; }

        public static Gene<T> From(Gene<T> from, Chromosome<T> parent)
        {
            return new Gene<T>(parent, from.Value);
        }
    }
}
