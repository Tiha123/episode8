using UnityEngine;
using System.Collections;
using MoreMountains.Feedbacks;

public class CollectableCoin : Collectable
{
    public uint addVal = 1;
    [SerializeField] Transform pivot;
    [SerializeField] MMF_Player feedbackdisappear;
    public override void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr)
    {
        laneNum = Mathf.Clamp(laneNum, 0, trackmgr.laneList.Count - 1);
        Transform laneTransform = trackmgr.laneList[laneNum];
        Vector3 spawnPosition = new Vector3(laneTransform.position.x, laneTransform.position.y, zpos);
        transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
    }
    public override void Collect()
    {
        
        GameManager.Coin += addVal;
        this.transform.SetParent(null);
        feedbackdisappear.PlayFeedbacks();
        
    }
}
