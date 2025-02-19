using UnityEngine;

public class ObstacleDouble : Obstacle
{
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
        if (laneNum >= trackmgr.laneList.Count - 1)
        {
            laneNum = Random.Range(0, laneNum);
        }
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
}
