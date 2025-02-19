using UnityEngine;

public class ObstacleTriple : Obstacle
{
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        Transform laneTransform = trackmgr.laneList[trackmgr.laneList.Count/2];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
}
