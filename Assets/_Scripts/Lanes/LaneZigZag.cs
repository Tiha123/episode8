
using UnityEngine;

public class LaneZigZag: Lane
{
    public string Name => "ZigZagPattern";
    LaneData data;
    public float ampX=0.8f; //진폭
    public float freq=2.5f;
    private int elapsed;
    private System.Random random=new System.Random();
    public void Initialize(int maxlane)
    {
        data.maxLane=maxlane;
    }
    public LaneData GetNextLane()
    {
        data.currentY=0f;
        data.currentLane=(int)Mathf.PingPong(elapsed++, data.maxLane-1);
        return data;
    }
}