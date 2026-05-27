using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public static WaypointPath Instance;
    public List<Transform> waypoints = new List<Transform>();

    private void Awake()
    {
        Instance = this;
        waypoints.Clear();
        foreach (Transform child in transform)
            waypoints.Add(child);
    }

    public Vector3 GetPoint(int index)
    {
        if (index < 0 || index >= waypoints.Count) return transform.position;
        return waypoints[index].position;
    }

    public int Count => waypoints.Count;
}
