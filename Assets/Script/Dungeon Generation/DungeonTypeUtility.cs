using System;

public enum DungeonType { SingleRoom, fullRoom, halfRoom, threeQuartRoom }

public static class DungeonTypeUtility
{
    public static DungeonType GetRandom()
    {
        var values = Enum.GetValues(typeof(DungeonType));
        return (DungeonType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}
