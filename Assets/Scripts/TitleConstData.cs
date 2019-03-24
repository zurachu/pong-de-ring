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
        public static readonly string ExpertStartForce = "ExpertStartForce";
        public static readonly string ExpertStartForceBaseNewBall = "ExpertStartForceBaseNewBall";
        public static readonly string ExpertStartForceAdditionalPerNewBall = "ExpertStartForceAdditionalPerNewBal";
        public static readonly string MaxVelocity = "MaxVelocity";
        public static readonly string NormalStartForce = "NormalStartForce";
        public static readonly string NormalStartForceBaseNewBall = "NormalStartForceBaseNewBall";
        public static readonly string NormalStartForceAdditionalPerNewBall = "NormalStartForceAdditionalPerNewBall";
        public static readonly string TweetMessage = "TweetMessage";
        public static readonly string TweetScoreFormat = "TweetScoreFormat";
    }

    public int BoundCountToOneUpBonus { get; private set; }
    public int BoundScoreBase { get; private set; }
    public int CoinScoreBase { get; private set; }
    public int ExpertInitialScore { get; private set; }
    public int ExpertInitialGotCoinCount { get; private set; }
    public float ExpertStartForce { get; private set; }
    public float ExpertStartForceBaseNewBall { get; private set; }
    public float ExpertStartForceAdditionalPerNewBall { get; private set; }
    public float MaxVelocity { get; private set; }
    public float NormalStartForce { get; private set; }
    public float NormalStartForceBaseNewBall { get; private set; }
    public float NormalStartForceAdditionalPerNewBall { get; private set; }
    public string TweetMessage { get; private set; }
    public string TweetScoreFormat { get; private set; }

    public TitleConstData(Dictionary<string, string> source)
    {
        if (source == null)
        {
            return;
        }

        BoundCountToOneUpBonus = GetValue<int>(source, ValueKey.BoundCountToOneUpBonus);
        BoundScoreBase = GetValue<int>(source, ValueKey.BoundScoreBase);
        CoinScoreBase = GetValue<int>(source, ValueKey.CoinScoreBase);
        ExpertInitialScore = GetValue<int>(source, ValueKey.ExpertInitialScore);
        ExpertInitialGotCoinCount = GetValue<int>(source, ValueKey.ExpertInitialGotCoinCount);
        ExpertStartForce = GetValue<float>(source, ValueKey.ExpertStartForce);
        ExpertStartForceBaseNewBall = GetValue<float>(source, ValueKey.ExpertStartForceBaseNewBall);
        ExpertStartForceAdditionalPerNewBall = GetValue<float>(source, ValueKey.ExpertStartForceAdditionalPerNewBall);
        MaxVelocity = GetValue<float>(source, ValueKey.MaxVelocity);
        NormalStartForce = GetValue<float>(source, ValueKey.NormalStartForce);
        NormalStartForceBaseNewBall = GetValue<float>(source, ValueKey.NormalStartForceBaseNewBall);
        NormalStartForceAdditionalPerNewBall = GetValue<float>(source, ValueKey.NormalStartForceAdditionalPerNewBall);
        TweetMessage = GetString(source, ValueKey.TweetMessage);
        TweetScoreFormat = GetString(source, ValueKey.TweetScoreFormat);
    }

    string GetString(Dictionary<string, string> source, string key)
    {
        if (source.TryGetValue(key, out var value))
        {
            return value.Replace("<br>", "\n");
        }
        return string.Empty;
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
