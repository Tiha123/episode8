using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum ObstacleType {Single, Top, Bottom, Double, Triple, _MAX}

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Obstacle obstaclePrefab;

    TrackManager trackmgr;

    [SerializeField] Transform spawnPoint;

    [SerializeField] float spawnInterval;

    [SerializeField] List<Obstacle> obstacleSingle;
    [SerializeField] List<Obstacle> obstacleTop;
    [SerializeField] List<Obstacle> obstacleBottom;
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
        yield return new WaitUntil(()=>GameManager.IsPlaying == true);
        StartCoroutine(SpawnInfinite());
    }

    IEnumerator SpawnInfinite()
    {
        float PrevDistance=GameManager.MoveDistance;
        while(true)
        {
            yield return new WaitUntil(()=>GameManager.IsPlaying);
            // if(GameManager.IsPlaying==false)
            // {
            //     yield return null;
            // }
            SpawnObstacle(UnityEngine.Random.Range(0,trackmgr.laneList.Count));
            yield return new WaitUntil(()=>(GameManager.MoveDistance-PrevDistance)>spawnInterval);
            PrevDistance=GameManager.MoveDistance;
        }
    }

    void SpawnObstacle(int laneNum)
    {
        laneNum = math.clamp(laneNum, 0, trackmgr.laneList.Count);
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, spawnPoint.position.z);
        Track t=trackmgr.GetTrackByZ(spawnPoint.position.z);
        if(t==null)
        {
            return;
        }
        Obstacle rndobstacle=RnadomTypeSpawn();
        if (rndobstacle!=null)
        {   
            Obstacle o = Instantiate(rndobstacle, spawnPosition, Quaternion.identity, t.ObstacleRoot);
        }
    }
    
    Obstacle RnadomTypeSpawn()
    {
        ;
        int rndType=UnityEngine.Random.Range((int)ObstacleType.Single, (int)ObstacleType._MAX);
        List<Obstacle> obstacles = rndType switch
        {
            (int)ObstacleType.Single=>obstacleSingle,
            (int)ObstacleType.Top=>obstacleTop,
            (int)ObstacleType.Bottom=>obstacleBottom,
            _=>null
        };
        if (obstacles==null)
        {
            return null;
        }
        return obstacles[UnityEngine.Random.Range(0,obstacles.Count)];
    }
}