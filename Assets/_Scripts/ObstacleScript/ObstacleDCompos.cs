using UnityEngine;
using System.Collections.Generic;

public class ObstacleDCompose : ObstacleDouble
{
    [SerializeField] List<Obstacle> compositePrefabs;

    protected List<Vector3> spawnedPos;

    void Start()
    {
        SpawnComposited();
    }
    protected void SpawnComposited()
    {

        spawnedPos.ForEach(p =>
        {
            int rnd = Random.Range(0, compositePrefabs.Count);
            Obstacle prefab = compositePrefabs[rnd];

            var o = Instantiate(prefab, p, Quaternion.identity, transform);
            Vector3 localPos = o.transform.localPosition;
            o.transform.localPosition = new Vector3(localPos.x, 0f, 0f);
        });

    }
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        int b=Random.Range(laneNum+1,trackmgr.laneList.Count);
        spawnedPos.Add(trackmgr.laneList[laneNum].position);
        spawnedPos.Add(trackmgr.laneList[b].position);
        
        Transform laneTransform = trackmgr.laneList[trackmgr.laneList.Count/2];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
}
