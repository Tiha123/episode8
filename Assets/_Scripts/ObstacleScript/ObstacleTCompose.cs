using UnityEngine;

public class ObstacleTCompose : ObstacleDCompose
{
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        foreach(Transform t in trackmgr.laneList)
        {
            spawnedPos.Add(t.position);
        }
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
}
