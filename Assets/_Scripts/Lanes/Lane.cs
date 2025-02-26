
public interface Lane
{
    public string Name {get;}
    public LaneData GetNextLane();
    public void Initialize(int maxlane);
}