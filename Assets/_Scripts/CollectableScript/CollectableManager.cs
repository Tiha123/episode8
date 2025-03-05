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
    public LaneType pattertype;
    public override object GetItem()
    {
        return pattertype;
    }
}

public class CollectableManager : MonoBehaviour
{
    RandomGenerator collectableGenerator = new RandomGenerator();
    

    [SerializeField] CollectableSO collectableData;
    
    [SerializeField] float spawnZpos = 18f;

    [SerializeField, AsRange(0, 100)] public Vector2 spawnInterval;
    [SerializeField, AsRange(1, 30)] public Vector2 spawnquota;
    

    [Space(20)]
    TrackManager trackmgr;
    LaneGenerator laneGen;
    int lanecount;

    void Awake()
    {

    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => collectableData!=null);
        collectableData.collectablePools.ForEach(pools =>
        {
            collectableGenerator.AddItem(pools);
        });
        trackmgr = FindFirstObjectByType<TrackManager>();
        lanecount=trackmgr.laneList.Count;
        laneGen = new LaneGenerator(collectableData.spawnquota, lanecount, collectableData.lanepatternPools);
        yield return new WaitUntil(() => GameManager.IsPlaying == true);
        StartCoroutine(SpawnInfinite());
    }

    IEnumerator SpawnInfinite()
    {
        float PrevDistance = GameManager.MoveDistance;
        while (true)
        {
            yield return new WaitUntil(() => GameManager.IsPlaying==true && GameManager.IsUIOpen==false);
            yield return new WaitUntil(() => collectableData!=null);
            Spawncollectable();
            yield return new WaitUntil(() => (GameManager.MoveDistance - PrevDistance) > Random.Range(collectableData.spawnInterval.x, collectableData.spawnInterval.y));
            PrevDistance = GameManager.MoveDistance;
        }
    }

    void Spawncollectable()
    {
        if(collectableData==null)
        {
            return;
        }
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
        if(phase.collectableSO==null)
        {
            Clear();
            return;
        }

        collectableData=phase.collectableSO;
        collectableGenerator.Clear();

        collectableData.collectablePools.ForEach(v=>collectableGenerator.AddItem(v));
        laneGen = new LaneGenerator(collectableData.spawnquota, lanecount, collectableData.lanepatternPools);
        DOVirtual.Vector2(spawnInterval, phase.collectableSO.spawnInterval, 1f, i=>spawnInterval=i);
    }

    void Clear()
    {
        collectableGenerator.Clear();
        collectableData=null;
    }
}
