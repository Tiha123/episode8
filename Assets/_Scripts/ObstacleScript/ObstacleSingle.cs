
using UnityEngine;

public enum SingleType {Top,Bottom, BlockAll, _None}


public class ObstacleSingle : Obstacle
{
    public SingleType thisType=SingleType._None;
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
}
