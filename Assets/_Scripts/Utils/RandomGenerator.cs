using System.Collections.Generic;
using UnityEngine;

public abstract class RandomItem
{
    public string name;
    public int weight;

    public abstract object GetItem();
}
public class RandomGenerator
{
    public List<RandomItem> items=new List<RandomItem> ();

    public int totalweight;

    private void CalcTotalWeight()
    {
        totalweight=0;
        items.ForEach(item=>
            {
                totalweight+=item.weight;
            });
    }

    public RandomItem GetRandom()
    {
        int rnd = Random.Range(0,totalweight);
        int weightSum=0;
        foreach (RandomItem item in items)
        {
            weightSum+=item.weight;
            if(rnd<weightSum)
            {
                return item;
            }
        }
        return null;
    }


    public void AddItem(RandomItem item)
    {
        items.Add(item);
        CalcTotalWeight();
    }

    public void Clear()
    {
        items.Clear();
        totalweight=0;
    }
}
