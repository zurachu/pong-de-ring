using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TitleConstData
{
    static class ValueKey
    {
        public static readonly string BoundCountToOneUpBonus = "BoundCountToOneUpBonus";
        public static readonly string BoundScoreBase = "BoundScoreBase";
        public static readonly string CoinScoreBase = "CoinScoreBase";
        public static readonly string ExpertInitialScore = "ExpertInitialScore";
        public static readonly string ExpertInitialGotCoinCount = "ExpertInitialGotCoinCount";
        public static readonly string ExpertStartForceBase = "ExpertStartForceBase";
        public static readonly string MaxVelocity = "MaxVelocity";
        public static readonly string NormalStartForceBase = "NormalStartForceBase";
        public static readonly string StartForceAdditionalPerNewBall = "StartForceAdditionalPerNewBall";
    }

    public int BoundCountToOneUpBonus { get; private set; }
    public int BoundScoreBase { get; private set; }
    public int CoinScoreBase { get; private set; }
    public int ExpertInitialScore { get; private set; }
    public int ExpertInitialGotCoinCount { get; private set; }
    public float ExpertStartForceBase { get; private set; }
    public float MaxVelocity { get; private set; }
    public float NormalStartForceBase { get; private set; }
    public float StartForceAdditionalPerNewBall { get; private set; }

    public TitleConstData(Dictionary<string, string> source)
    {
        BoundCountToOneUpBonus = GetValue<int>(source, ValueKey.BoundCountToOneUpBonus);
        BoundScoreBase = GetValue<int>(source, ValueKey.BoundScoreBase);
        CoinScoreBase = GetValue<int>(source, ValueKey.CoinScoreBase);
        ExpertInitialScore = GetValue<int>(source, ValueKey.ExpertInitialScore);
        ExpertInitialGotCoinCount = GetValue<int>(source, ValueKey.ExpertInitialGotCoinCount);
        ExpertStartForceBase = GetValue<float>(source, ValueKey.ExpertStartForceBase);
        MaxVelocity = GetValue<float>(source, ValueKey.MaxVelocity);
        NormalStartForceBase = GetValue<float>(source, ValueKey.NormalStartForceBase);
        StartForceAdditionalPerNewBall = GetValue<float>(source, ValueKey.StartForceAdditionalPerNewBall);
    }

    T GetValue<T>(Dictionary<string, string> source, string key)
    {
        if (source.TryGetValue(key, out var value))
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFromString(value);
            }
        }
        return default(T);
    }
}
