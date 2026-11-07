using System.Collections.Generic;
using UnityEngine;

public sealed class WaveViewCollection
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Medium medium;
    private readonly Medium oppositeMedium;

    private readonly Dictionary<int, GameObject> views = new();

    public WaveViewCollection(
        GameObject prefab,
        Transform parent,
        Medium medium,
        Medium oppositeMedium
    )
    {
        this.prefab = prefab;
        this.parent = parent;
        this.medium = medium;
        this.oppositeMedium = oppositeMedium;
    }

    public void CreateViews(List<int> waveIds)
    {
        foreach(int id in waveIds)
        {
            if(views.ContainsKey(id)) continue;

            GameObject view = Object.Instantiate(prefab);

            view.GetComponent<OneWaveLenderer>().Init(medium, oppositeMedium, id);

            views.Add(id,view);
        }
    }

    public void RemoveMissingViews()
    {
        List<int> idsToRemove = new();

        foreach(var pair in views)
        {
            if(medium.waveIds.Contains(pair.Key)) continue;

            if(pair.Key == medium.nextWaveId) continue;

            Object.Destroy(pair.Value);
            idsToRemove.Add(pair.Key);
        }

        foreach(int id in idsToRemove)
        {
            views.Remove(id);
        }
    }
}