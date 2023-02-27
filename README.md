# GPU driven Genetic Algorithm (Unity/.NET)

[...]

# Using

## Performers (operations)

### Evaluation

Perform the fitness function over the population.
Stores the fitness results into the Chromosome.

### Picking stages

#### Elite stage 
  Pass the best chromosomes (defined as `chromosomes_count * elite_factor`) from previous generation to the next one. No crossover is applied at this stage.
  
#### Tournement stage
  * Take 2 distinct chromosomes, define the winner as `best_1`
  * Take 2 other distinct chromosomes, define the winner as `best_2`
  * Apply crossover to `best_1` and `best_2`, obtaining `child_1` and `child_2`
  * Repeat until the new population is fully completed
  
### Mutation

#### Outer swapping (GPU-ready)
  Swaps values from the pool and genes (inserts new values).
  
#### Inner swapping (GPU-ready)
  Swaps values of the genes (re-orders some of the genes).
