using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();
    public Lane currentPattern;
    private Vector2 limitQuotaVec;
    private int limitQuota;
    private int currentQuota=0;
    private int laneCout;
    private System.Random random=new System.Random();

    public LaneGenerator(Vector2 quota, int lanecount)
    {
        limitQuotaVec=quota;
        limitQuota=random.Next((int)limitQuotaVec.x,(int)limitQuotaVec.y);
        lanePatterns.Add(new LaneStraight());
        lanePatterns.Add(new LaneWave());
        lanePatterns.Add(new LaneZigZag());
        laneCout=lanecount;
        SwitchPattern();
        currentPattern.Initialize(laneCout);
    }

    public LaneData GetNextLane()
    {
        if (currentPattern == null)
        {
            return new LaneData();
        }
        currentQuota++;
        if(currentQuota>=limitQuota)
        {
            SwitchPattern();
        }
        return currentPattern.GetNextLane();
    }
    public void SwitchPattern(int index = -1)
    {
        // -1: 랜덤
        index = index == -1 ? Random.Range(0, lanePatterns.Count) : Mathf.Clamp(index, 0, lanePatterns.Count - 1);
        limitQuota=random.Next((int)limitQuotaVec.x,(int)limitQuotaVec.y);
        Lane lane = lanePatterns[index];
        currentPattern = lane;
        currentPattern.Initialize(laneCout);
        currentQuota=0;
    }
}
