using UnityEngine;
using System.Collections.Generic;

public class ObstacleDCompose : Obstacle
{
    [SerializeField] protected List<ObstacleSingle> compositePrefabs;

    protected List<Vector3> spawnedPos=new List<Vector3>();

    protected virtual void SpawnComposited()
    {

        spawnedPos.ForEach(p =>
        {
            ObstacleSingle Singleprefab = GetRandomPrefab(compositePrefabs);

            Spawn(Singleprefab,p);
        });

    }
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
        int a=Random.Range(0,trackmgr.laneList.Count-1);
        int b=Random.Range(a+1,trackmgr.laneList.Count);
        spawnedPos.Add(trackmgr.laneList[a].position);
        spawnedPos.Add(trackmgr.laneList[b].position);
        
        Transform laneTransform = trackmgr.laneList[trackmgr.laneList.Count/2];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        SpawnComposited();
    }
    private ObstacleSingle GetRandomPrefab(List<ObstacleSingle> prefabs)
    {
        if(prefabs==null||prefabs.Count==0)
        {
            return null;
        }
        int rnd=Random.Range(0,prefabs.Count);
        ObstacleSingle result=prefabs[rnd];
        return result;
    }
    private void Spawn(Obstacle prefab, Vector3 pos)
    {
        var o = Instantiate(prefab, pos, Quaternion.identity, transform);
        Vector3 localPos = o.transform.localPosition;
        o.transform.localPosition = new Vector3(localPos.x, 0f, 0f);
    }
}
