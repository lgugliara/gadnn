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
    public int best_score = 19_913_031;
    public int chromosomes_count = 20;
    public int genes_count = 10000;
    public double mutation_factor = 0.1;
    public bool isUnique = true;

    Thread t;

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

    void BestChangeEvent(object sender, EventArgs e)
    {
        FileHandler.DrawAdam(
            @"Assets/ReplyChallenges/2022/Out/" + file_name + ".txt",
            (sender as Population<int>).Best
        );
    }

    void Start()
    {
        FileHandler.ImportInputData(@"Assets/ReplyChallenges/2022/In/" + file_name + ".txt");

        var values = GameParameter.Demons.Select(x => x.Id).ToList();

        //FileHandler.ImportAdamData(@"Assets/ReplyChallenges/2022/Out/" + file_name + ".txt", adam);

        var ga = new GeneticAlgorithm<int>(
            chromosomes_count: chromosomes_count,
            genes_count: genes_count,
            values: values,
            evaluate: evaluate,
            comparer: best_score,
            mutation_factor: mutation_factor,
            isUnique: isUnique,
            on_best_change: BestChangeEvent
        );

        t = new Thread(() => ga.Start());
        t.Start();
    }

    void OnApplicationQuit()
    {
        t.Abort();
    }
}