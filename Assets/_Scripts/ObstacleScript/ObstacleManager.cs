using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] Obstacle obstaclePrefab;

    TrackManager trackmgr;

    [SerializeField] Transform spawnPoint;

    [SerializeField] float spawnInterval;

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
        while(true)
        {
            if(GameManager.IsPlaying==false)
            {
                yield break;
            }
            SpawnObstacle(UnityEngine.Random.Range(0,trackmgr.laneList.Count-1));
            yield return new WaitForSeconds(spawnInterval);
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
        Obstacle o = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity, t.ObstacleRoot);
    }
}