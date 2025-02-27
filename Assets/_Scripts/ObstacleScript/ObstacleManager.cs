using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
using DG.Tweening;

[System.Serializable]
public class ObstaclePool : RandomItem
{
    public List<Obstacle> obstacleList;

    public override object GetItem()
    {
        if (obstacleList==null || obstacleList.Count<=0)
            return null;
        return obstacleList[Random.Range(0, obstacleList.Count)];
    }
}

public class ObstacleManager : MonoBehaviour
{
    public List<ObstaclePool> obstaclePools;
    [SerializeField, AsRange(0,100)] Vector2 spawnIntervalo;
    [SerializeField] float spawnZpos = 18f;

    [Space(20)]
    TrackManager trackmgr;
    RandomGenerator rdm = new RandomGenerator();

    // [SerializeField] List<Obstacle> obstacleDouble;
    // [SerializeField] List<Obstacle> obstacleSingle;
    // [SerializeField] List<Obstacle> obstacleTriple;
    // [SerializeField] List<Obstacle> obstacleDComposite;
    // [SerializeField] List<Obstacle> obstacleTComposite;

    IEnumerator Start()
    {
        obstaclePools.ForEach(pools =>
        {
            rdm.AddItem(pools);
        });
        // TrackManager[] tm = FindObjectsByType<TrackManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        // if (tm == null || tm.Length <= 0)
        // {
        //     yield break;
        // }
        // trackmgr = tm[0];

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
            // if(GameManager.IsPlaying==false)
            // {
            //     yield return null;
            // }
            SpawnObstacle();
            yield return new WaitUntil(() => (GameManager.MoveDistance - PrevDistance) > Random.Range(spawnIntervalo.x,spawnIntervalo.y));
            PrevDistance = GameManager.MoveDistance;
        }
    }

    void SpawnObstacle()
    {
        (int laneNum, Obstacle rndobstacle) = RandomLanePrefab();


        Track t = trackmgr.GetTrackByZ(spawnZpos);

        if (t == null)
        {
            return;
        }
        if (rndobstacle != null)
        {
            Obstacle o = Instantiate(rndobstacle, t.ObstacleRoot);
            o.SetLandPosition(laneNum, spawnZpos, trackmgr);
        }
    }

    (int, Obstacle) RandomLanePrefab()
    {
        int rndLane = Random.Range(0, trackmgr.laneList.Count);
        // int rndType = Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX);
        Obstacle obstacle = rdm.GetRandom().GetItem() as Obstacle;
        // List<Obstacle> obstacles = rndType switch
        // {
        //     (int)ObstacleType.Single => obstacleSingle,
        //     (int)ObstacleType.Double => obstacleDouble,
        //     (int)ObstacleType.Triple => obstacleTriple,
        //     (int)ObstacleType.DoubleComposite => obstacleDComposite,
        //     (int)ObstacleType.TripleComposite => obstacleTComposite,
        //     _ => null
        // };
        Obstacle prefab;
        if (obstacle != null)
        {
            prefab = obstacle;
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

    public void SetPhase(Vector2 newInterval, List<int> newWeight)
    {
        DOVirtual.Vector2(spawnIntervalo, newInterval, 1f, i=>spawnIntervalo=i);
        rdm.AdjustWeight(newWeight);
    }

}