using System;
using System.Collections.Generic;

namespace GeneticTspSolver
{
    public class Gene<T>
    {
        public Chromosome<T> Parent { get; set; }
        public int Id { get; private set; }

        public T Value
        {
            get => Parent.Values[Id];
            set => Parent.Values[Id] = value;
        }

        public Gene(Chromosome<T> parent, int id, T value)
        {
            Parent = parent;
            Id = id;

            Value = value;
        }

        public override string ToString() => Value.ToString();

        public static Gene<T> From(Gene<T> from, Chromosome<T> parent)
        {
            return new Gene<T>(parent, from.Id, from.Value);
        }
    }
}
