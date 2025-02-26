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
        if (collectable == null)
            return null;
        return collectable;
    }
}

// [System.Serializable]
// public class LanePool : RandomItem
// {
//     public Lane lane;
//     public override Object GetItem()
//     {
//         if (lane == null)
//             return null;
//         return lane;
//     }
// }

public class CollectableManager : MonoBehaviour
{
    public List<CollectablePool> collectablePools;
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;
    [SerializeField] float spawnZpos = 18f;
    [SerializeField, AsRange(1, 30)] Vector2 spawnquota;

    [Space(20)]
    TrackManager trackmgr;
    LaneGenerator laneGen;
    RandomGenerator rdm = new RandomGenerator();

    void Awake()
    {

    }

    IEnumerator Start()
    {
        collectablePools.ForEach(pools =>
        {
            rdm.AddItem(pools);
        });

        trackmgr = FindFirstObjectByType<TrackManager>();
        yield return new WaitForEndOfFrame();
        laneGen = new LaneGenerator(spawnquota, trackmgr.laneList.Count);
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
            yield return new WaitUntil(() => (GameManager.MoveDistance - PrevDistance) > Random.Range(spawnInterval.x, spawnInterval.y));
            PrevDistance = GameManager.MoveDistance;
        }
    }

    void Spawncollectable()
    {
        (LaneData laneCurrent, Collectable rndcollectable) = RandomLanePrefab();


        Track t = trackmgr.GetTrackByZ(spawnZpos);

        if (t == null)
        {
            return;
        }
        if (rndcollectable != null)
        {
            Collectable o = Instantiate(rndcollectable, t.collectableRoot);
            o.SetLandPosition(laneCurrent.currentLane, laneCurrent.currentY, spawnZpos, trackmgr);
        }
    }

    (LaneData, Collectable) RandomLanePrefab()
    {

        LaneData rndLane = laneGen.GetNextLane();
        Collectable collectable = rdm.GetRandom().GetItem() as Collectable;
        Collectable prefab;
        if (collectable != null)
        {
            prefab = collectable;
        }
        else
        {
            prefab = null;
        }
        if (prefab == null)
        {
            return (new LaneData(), null);
        }
        return (rndLane, prefab);
    }
}
