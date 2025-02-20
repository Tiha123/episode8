
using UnityEngine;

//추상 클래스
public abstract class Obstacle : MonoBehaviour
{
    public abstract void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr);
    // {
    //     laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
    //     Transform laneTransform = trackmgr.laneList[laneNum];
    //     Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
    //     transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    // }
}
