using GeneticTspSolver;

namespace GeneticTspSolver
{
    public class EndlessTermination<T> : ITermination<T>
    {
        public bool IsTerminated(IGeneticAlgorithm<T> geneticAlgorithm) => false;
    }
    
    public interface ITermination<T>
    {
        bool IsTerminated(IGeneticAlgorithm<T> geneticAlgorithm);
    }
}
