using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public abstract void SetLandPosition(int laneNum,float laneY, float zpos, TrackManager trackmgr);

    public abstract void Collect();
}
