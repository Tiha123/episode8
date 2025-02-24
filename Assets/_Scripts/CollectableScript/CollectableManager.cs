using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Collections;

[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;
    public override Object GetItem()
    {
        if (collectable==null)
            return null;
        return collectable;
    }
}

public class CollectableManager : MonoBehaviour
{
    public List<CollectablePool> collectablePools;
    [SerializeField, AsRange(0,100)] Vector2 spawnInterval;
    [SerializeField] float spawnZpos = 18f;

    [Space(20)]
    TrackManager trackmgr;
    RandomGenerator rdm = new RandomGenerator();

    IEnumerator Start()
    {
        collectablePools.ForEach(pools =>
        {
            rdm.AddItem(pools);
        });

        trackmgr=FindFirstObjectByType<TrackManager>();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => GameManager.IsPlaying == true);
        StartCoroutine(SpawnInfinite());
    }

    IEnumerator SpawnInfinite()
    {
        float PrevDistance = GameManager.MoveDistance;
        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsPlaying);
            Spawncollectable();
            yield return new WaitUntil(() => (GameManager.MoveDistance - PrevDistance) > Random.Range(spawnInterval.x,spawnInterval.y));
            PrevDistance = GameManager.MoveDistance;
        }
    }

    void Spawncollectable()
    {
        (int laneNum, Collectable rndcollectable) = RandomLanePrefab();


        Track t = trackmgr.GetTrackByZ(spawnZpos);

        if (t == null)
        {
            return;
        }
        if (rndcollectable != null)
        {
            Collectable o = Instantiate(rndcollectable, t.collectableRoot);
            o.SetLandPosition(laneNum, spawnZpos, trackmgr);
        }
    }

    (int, Collectable) RandomLanePrefab()
    {
        int rndLane = Random.Range(0, trackmgr.laneList.Count);
        Collectable collectable = rdm.GetRandom(true).GetItem() as Collectable;
        Collectable prefab;
        if (collectable != null)
        {
            prefab = collectable;
        }
        else
        {
            prefab = null;
        }
        if (prefab==null)
        {
            return (-1, null);
        }
        return (rndLane, prefab);
    }
}
