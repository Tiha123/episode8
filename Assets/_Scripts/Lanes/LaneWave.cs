

using UnityEngine;

public class LaneWave : Lane
{
    public string Name => "WavePattern";
    LaneData data;
    public float ampY=1.5f; //진폭
    public float freq=2.5f;
    private float elapsed;
    private System.Random random=new System.Random();

    public void Initialize(int maxlane)
    {
        data.maxLane=maxlane;
        data.currentLane=random.Next(0,maxlane);
    }

    public LaneData GetNextLane()
    {
        elapsed+=0.1f;
        data.currentY=Mathf.Abs(Mathf.Sin(elapsed*Mathf.PI*freq)*ampY);
        return data;
    }
    
    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     Vector3 v1;
    //     float t;
    //     for(int i=0;i<=(count+1);++i)
    //     {
    //         t=(float)i/(float)(count+1);
    //         v1 = Vector3.Lerp(transform.position, transform.position+transform.forward*offsetZ, t);
    //         
    //         v1=new Vector3(v1.x,v1.y+s,v1.z);
    //         Gizmos.DrawCube(v1,Vector3.one*0.5f);
    //     }
    // }
}
