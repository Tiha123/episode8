using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public abstract void SetLandPosition(int laneNum, float zpos, TrackManager trackmgr);

    public abstract void Collect();
}
