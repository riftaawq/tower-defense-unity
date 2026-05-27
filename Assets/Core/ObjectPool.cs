using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int initialSize = 20;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        Warmup();
    }

    public void Warmup()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            var extra = Instantiate(prefab, transform);
            extra.SetActive(false);
            pool.Enqueue(extra);
        }

        var result = pool.Dequeue();
        result.SetActive(true);
        return result;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
