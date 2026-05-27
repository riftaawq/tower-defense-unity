using System.Collections.Generic;
using UnityEngine;

public class WaveBuilderAI : MonoBehaviour
{
    public List<EnemyData> enemyTypes = new List<EnemyData>();
    public EconomyManager economy;
    public int maxEnemiesPerWave = 50;

    public List<EnemyData> BuildWave(int round)
    {
        List<EnemyData> result = new List<EnemyData>();

        if (economy == null || enemyTypes == null || enemyTypes.Count == 0)
            return result;

        int budget = economy.AttackBudget;

        enemyTypes.Sort((a, b) => a.cost.CompareTo(b.cost));

        while (budget >= enemyTypes[0].cost && result.Count < maxEnemiesPerWave)
        {
            EnemyData chosen = ChooseEnemyForBudget(budget, round);
            if (chosen == null) break;

            result.Add(chosen);
            budget -= chosen.cost;
        }

        return result;
    }

    private EnemyData ChooseEnemyForBudget(int budget, int round)
    {
        List<EnemyData> available = new List<EnemyData>();

        foreach (var enemy in enemyTypes)
        {
            if (enemy != null && enemy.cost <= budget)
                available.Add(enemy);
        }

        if (available.Count == 0) return null;

        if (round < 3)
            return available[0];

        if (round < 6)
            return available[Random.Range(0, available.Count)];

        available.Sort((a, b) => b.cost.CompareTo(a.cost));
        return available[0];
    }
}