using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntryView : MonoBehaviour
{
    [SerializeField] GameObject defaultBackground;
    [SerializeField] GameObject playerBackground;
    [SerializeField] Text rank;
    [SerializeField] Text playerName;
    [SerializeField] Text score;

    public void Initialize(PlayerLeaderboardEntry entry)
    {
        var isMyEntry = (PlayFabLoginManagerSingleton.Instance.PlayFabId == entry.PlayFabId);
        defaultBackground.SetActive(!isMyEntry);
        playerBackground.SetActive(isMyEntry);

        rank.text = (entry.Position + 1).ToString();
        playerName.text = entry.DisplayName;
        score.text = entry.StatValue.ToString();
    }
}
