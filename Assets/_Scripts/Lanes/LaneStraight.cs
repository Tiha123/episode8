public class LaneStraight : Lane
{
    public string Name => "StraightPattern";
    private LaneData data;
    private System.Random random=new System.Random();

    public void Initialize(int maxlane)
    {
        data.maxLane=maxlane;
        //data.currentLane = UnityEngine.Random.Range(0,maxlane);
        data.currentLane=random.Next(0,maxlane);
    }

    public LaneData GetNextLane()
    {
        data.currentY=0f;
        return data;
    }
    
}
