using System.Collections.Generic;
using UnityEngine;

public class LaneGenerator
{
    private List<Lane> lanePatterns = new List<Lane>();
    public Lane currentPattern;
    public RandomGenerator rdm=new RandomGenerator();
    private Vector2 limitQuotaVec;
    private int limitQuota;
    private int currentQuota;
    private int laneCout;
    private System.Random random=new System.Random();

    public int totalweight;
    public LaneGenerator(Vector2 quota, int lanecount, List<LanePool> lanepools)
    {
        limitQuotaVec=quota;
        currentQuota=0;

        limitQuota=random.Next((int)limitQuotaVec.x,(int)limitQuotaVec.y);
        lanePatterns.Add(new LaneStraight());
        lanePatterns.Add(new LaneWave());
        lanePatterns.Add(new LaneZigZag());

        lanepools.ForEach(v=>rdm.AddItem(v));

        laneCout=lanecount;

        SwitchPattern();

        currentPattern?.Initialize(laneCout);

    }


    public LaneData GetNextLane()
    {
        currentQuota++;
        if(currentQuota>=limitQuota)
        {
            SwitchPattern();
        }

        if (currentPattern == null)
        {
            LaneData nullData=new LaneData(-1);
            return nullData;
        }
        else
        {
            return currentPattern.GetNextLane();
        }
    }
    public void SwitchPattern()
    {
        // -1: 랜덤
        //index = Random.Range(0, lanePatterns.Count);
        //Lane lane = lanePatterns[index];
        string patterName=rdm.GetRandom().GetItem() as string;

        Lane lane=lanePatterns.Find(f=>f.Name==patterName);

        limitQuota=random.Next((int)limitQuotaVec.x,(int)limitQuotaVec.y);

        currentPattern = lane;
        currentPattern?.Initialize(laneCout);
        currentQuota=0;
    }
    
}
