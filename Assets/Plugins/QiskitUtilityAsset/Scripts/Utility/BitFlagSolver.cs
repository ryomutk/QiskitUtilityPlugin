using System;

public static class BitFlagSolver
{
    public static bool HasFlag(string bitflag, int targetBit)
    {
        var flag = Convert.ToInt32(bitflag, 2);
        return HasFlag(flag,targetBit);
    }

    public static bool HasFlag(int bitFlag, int targetBit)
    {
        var result = bitFlag & targetBit;

        return result == targetBit;
    }
}