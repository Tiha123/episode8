
using System;
using System.Collections.Generic;

public static class IntExtension
{
    private static List<Tuple<int, string>> units = new List<Tuple<int, string>>()
    {
        new Tuple<int, string>(15, "Q"),
        new Tuple<int, string>(12, "T"),
        new Tuple<int, string>(9, "B"),
        new Tuple<int, string>(6, "M"),
        new Tuple<int, string>(3, "K"),
    };
    
    public static string ToStringKilo(this int num)
    {
        int zerocount = num.ToString().Length;
        for (int i = 0; i < units.Count; i++)
            if (zerocount > units[i].Item1)
            {
                double result = num / Math.Pow(10, units[i].Item1);
                return $"{Math.Floor(result * 100) / 100}<size=0.85%>{units[i].Item2}</size>";
            }

        return num.ToString();
    }
}
