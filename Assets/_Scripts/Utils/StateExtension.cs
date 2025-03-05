using System;
using System.Collections.Generic;

public static class StateExtension
{
    public static bool HasAny(this PlayerState flag, params PlayerState[] compares)
    {
        foreach(var c in compares)
        {
            if((flag & c) !=0)
            {
                return true;
            }
        }
        return false;
    }
}
