using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Pool Mapping")]
    public List<EnemyPoolBinding> pools = new List<EnemyPoolBinding>();
    public float spawnInterval = 1f;

    private readonly List<Enemy> aliveEnemies = new List<Enemy>();

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator SpawnWave(List<EnemyData> wave)
    {
        for (int i = 0; i < wave.Count; i++)
        {
            SpawnEnemy(wave[i]);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnEnemy(EnemyData data)
    {
        var pool = GetPool(data);
        if (pool == null) return;

        GameObject obj = pool.Get();
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Init(data);
        aliveEnemies.Add(enemy);
    }

    public void DespawnEnemy(Enemy enemy)
    {
        if (aliveEnemies.Contains(enemy))
            aliveEnemies.Remove(enemy);

        var pool = GetPool(enemy.data);
        if (pool != null)
            pool.ReturnToPool(enemy.gameObject);
        else
            enemy.gameObject.SetActive(false);
    }

    public bool HasAliveEnemies() => aliveEnemies.Count > 0;

    public List<Enemy> GetAliveEnemies()
    {
        aliveEnemies.RemoveAll(e => e == null || !e.gameObject.activeInHierarchy);
        return aliveEnemies;
    }

    private ObjectPool GetPool(EnemyData data)
    {
        foreach (var binding in pools)
        {
            if (binding.enemyData == data)
                return binding.pool;
        }
        return null;
    }
}

[System.Serializable]
public class EnemyPoolBinding
{
    public EnemyData enemyData;
    public ObjectPool pool;
}
