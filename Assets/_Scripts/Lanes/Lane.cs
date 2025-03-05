public enum LaneType {EMPTY, LaneStraight, LaneWave, LaneZigZag, _MAX}

public interface Lane
{
    public LaneType lanetype {get;}
    public LaneData GetNextLane();
    public void Initialize(int maxlane);
}