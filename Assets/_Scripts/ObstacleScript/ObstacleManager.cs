using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleType { Single, Double, Triple,DoubleComposite, _MAX }

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Obstacle obstaclePrefab;

    TrackManager trackmgr;
    [SerializeField] float spawnInterval;
    [SerializeField] List<Obstacle> obstacleSingle;
    [SerializeField] List<Obstacle> obstacleDouble;
    [SerializeField] List<Obstacle> obstacleTriple;
    [SerializeField] float spawnZpos = 18f;
    List<List<Obstacle>> obstacleList;

    IEnumerator Start()
    {
        TrackManager[] tm = FindObjectsByType<TrackManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (tm == null || tm.Length <= 0)
        {
            yield break;
        }

        yield return new WaitForEndOfFrame();
        trackmgr = tm[0];
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
            yield return new WaitUntil(() => (GameManager.MoveDistance - PrevDistance) > spawnInterval);
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
        int rndType = Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX);
        List<Obstacle> obstacles = rndType switch
        {
            (int)ObstacleType.Single => obstacleSingle,
            (int)ObstacleType.Double => obstacleDouble,
            (int)ObstacleType.Triple => obstacleTriple,
            (int)ObstacleType.DoubleComposite => obstacleTriple,
            _ => null
        };
        Obstacle prefab;
        if (obstacles.Count != 0 && obstacles != null)
        {
            prefab = obstacles[Random.Range(0, obstacles.Count)];
        }
        else
        {
            prefab = null;
        }
        return (rndLane, prefab);
    }
}