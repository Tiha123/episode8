using System.Collections.Generic;
using UnityEngine;

public class ObstacleTCompose : ObstacleDCompose
{
    int blockcount;
    List<ObstacleSingle> SafeOb = new List<ObstacleSingle>();

    void Start()
    {
        SafeOb = compositePrefabs.FindAll(f => f.thisType != SingleType.BlockAll);
    }
    protected override void SpawnComposited()
    {

        spawnedPos.ForEach(p =>
        {
            ObstacleSingle Singleprefab = GetRandomPrefab(compositePrefabs);
            if (Singleprefab.thisType == SingleType.BlockAll)
            {
                if (--blockcount < 1)
                {
                    Singleprefab = GetRandomPrefab(SafeOb);
                    Debug.Log("Active");
                }
            }
            Spawn(Singleprefab, p);
        });

    }
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        foreach (Transform t in trackmgr.laneList)
        {
            spawnedPos.Add(t.position);
        }
        Transform laneTransform = trackmgr.laneList[trackmgr.laneList.Count/2];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        blockcount = trackmgr.laneList.Count;
        SpawnComposited();
    }

    private void Spawn(Obstacle prefab, Vector3 pos)
    {
        var o = Instantiate(prefab, pos, Quaternion.identity, transform);
        Vector3 localPos = o.transform.localPosition;
        o.transform.localPosition = new Vector3(localPos.x, 0f, 0f);
    }

    private ObstacleSingle GetRandomPrefab(List<ObstacleSingle> prefabs)
    {
        int rnd = Random.Range(0, prefabs.Count);
        ObstacleSingle result = prefabs[rnd];
        return result;
    }
}
