
public struct LaneData
{
    public int maxLane;

    public int currentLane;

    public float currentY;

    public LaneData(int current=-1)
    {
        maxLane=current;
        currentLane=current;
        currentY=0;
    }
}
