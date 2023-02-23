using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GeneticTspSolver;
using ReplyChallenge2022;

// ensure class initializer is called whenever scripts recompile
[InitializeOnLoadAttribute]
public class Test : MonoBehaviour
{
    public string file_name = "5";
    public int population_size = 10;
    public int chromosome_size = 1000;

    Thread t;
    System.Random rnd = new System.Random();

    Func<Chromosome<int>, double> evaluate = (Chromosome<int> chromosome) =>
    {
        var currentTurn = 0;
        var score = 0;
        var currStamina = GameParameter.InitialStamina;
        var nextActionsToTake = new Dictionary<int, int>();
        int geneIndex = 0;
        var demon = GameParameter.Demons[(chromosome.Genes[geneIndex]).Value];

        while (currentTurn < GameParameter.Turns)
        {
            if (nextActionsToTake.ContainsKey(currentTurn))
            {
                currStamina = Math.Min(currStamina + nextActionsToTake[currentTurn], GameParameter.MaxStamina);
                nextActionsToTake.Remove(currentTurn);
            }
            if (currStamina >= demon.StaminaToDefeat)
            {
                currStamina -= demon.StaminaToDefeat;
                score += demon.Fragments.Take(GameParameter.Turns - currentTurn).Sum();

                if (!nextActionsToTake.ContainsKey(currentTurn + demon.TurnBeforeStamina))
                    nextActionsToTake.Add(currentTurn + demon.TurnBeforeStamina, 0);

                nextActionsToTake[currentTurn + demon.TurnBeforeStamina] += demon.StaminaRecovered;
                currentTurn++;
                geneIndex++;

                if (geneIndex >= chromosome.Genes.Count)
                    break;
                
                demon = GameParameter.Demons[(chromosome.Genes[geneIndex]).Value];
            }
            else
            {
                if (nextActionsToTake.Any())
                    currentTurn = nextActionsToTake.Min(x => x.Key);
                else
                    break;
            }
        }

        return score;
    };

    /* void Ga_BestChromosomeChanged(object? sender, EventArgs e)
    {
        FileHandler.DrawAdam(@"Assets/TSP/Reply/2022/Out/" + file_name + ".txt", ga.BestChromosome);
    } */
    
    void Start()
    {
        FileHandler.ImportInputData(@"Assets/TSP/Reply/2022/In/" + file_name + ".txt");

        var pool = GameParameter.Demons.Select(x => x.Id).ToList();
        var tmp_pool = new List<int>(pool);

        var values = Enumerable.Range(0, chromosome_size).Select(v => {
            var index = tmp_pool[rnd.Next(0, tmp_pool.Count)];
            tmp_pool.Remove(index);
            return index;
        }).ToArray();

        //FileHandler.ImportAdamData(@"Assets/TSP/Reply/2022/Out/" + file_name + ".txt", adam);

        var ga = new GeneticAlgorithm<int>(population_size, values, evaluate, pool.ToArray());

        t = new Thread(() => ga.Start());
        t.Start();
    }

    void OnApplicationQuit()
    {
        t.Abort();
    }
}