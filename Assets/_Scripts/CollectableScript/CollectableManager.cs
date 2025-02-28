using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public class CollectablePool : RandomItem
{
    public Collectable collectable;
    public override object GetItem()
    {
        if (collectable == null)
            return null;
        return collectable;
    }
}

[System.Serializable]
public class LanePool : RandomItem
{
    public string lane;
    public override object GetItem()
    {
        if (lane == null)
            return null;
        return lane;
    }
}

public class CollectableManager : MonoBehaviour
{
    RandomGenerator collectableGenerator = new RandomGenerator();
    public List<CollectablePool> collectablePools;
    public List<LanePool> lanepatternPools;
    [SerializeField, AsRange(0, 100)] Vector2 spawnInterval;
    [SerializeField] float spawnZpos = 18f;
    [SerializeField, AsRange(1, 30)] Vector2 spawnquota;

    [Space(20)]
    TrackManager trackmgr;
    LaneGenerator laneGen;

    void Awake()
    {

    }

    IEnumerator Start()
    {
        collectablePools.ForEach(pools =>
        {
            collectableGenerator.AddItem(pools);
        });

        trackmgr = FindFirstObjectByType<TrackManager>();
        yield return new WaitForEndOfFrame();
        laneGen = new LaneGenerator(spawnquota, trackmgr.laneList.Count, lanepatternPools);
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
        if (rndcollectable != null&&laneCurrent.currentLane!=-1)
        {
            Collectable o = Instantiate(rndcollectable, t.collectableRoot);
            o.SetLandPosition(laneCurrent.currentLane, laneCurrent.currentY, spawnZpos, trackmgr);
        }
    }

    (LaneData, Collectable) RandomLanePrefab()
    {

        LaneData rndLane = laneGen.GetNextLane();
        Collectable collectable = collectableGenerator.GetRandom().GetItem() as Collectable;
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
            return (new LaneData(-1), null);
        }
        return (rndLane, prefab);
    }

    public void SetPhase(PhaseSO phase)
    {
        DOVirtual.Vector2(spawnInterval, phase.collectableInterval, 1f, i=>spawnInterval=i);
        DOVirtual.Vector2(spawnquota, phase.collectableQuota, 1f, i=>spawnquota=i);
    }
}
