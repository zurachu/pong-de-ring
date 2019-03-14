using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDisplayNameView : MonoBehaviour
{
    static readonly int DisplayNameMinLength = 3;

    [SerializeField] Text playFabIdText;
    [SerializeField] InputField displayNameInputField;
    [SerializeField] Button okButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button closeButton;
    [SerializeField] ConnectingView connectingViewPrefab;

    Action<string> onChangeDisplayName;
    ConnectingView connectingView;

    public void Initialize(string displayName, Action<string> onChangeDisplayName)
    {
        this.onChangeDisplayName = onChangeDisplayName;

        playFabIdText.text = PlayFabLoginManagerSingleton.Instance.PlayFabId;
        displayNameInputField.text = displayName;

        if (string.IsNullOrEmpty(displayName))
        {
            var okButtonPosition = okButton.transform.localPosition;
            okButtonPosition.x = 0f;
            okButton.transform.localPosition = okButtonPosition;
            okButton.interactable = false;
            cancelButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
        }
    }

    public void OnValueChanged()
    {
        okButton.interactable = (displayNameInputField.text.Length >= DisplayNameMinLength);
    }

    public void OnEndEdit()
    {
        displayNameInputField.text = displayNameInputField.textComponent.text;
        OnValueChanged();
    }

    public void OnClickOk()
    {
        connectingView = ConnectingView.Show();

        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = displayNameInputField.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateSuccess, OnUpdateFailure);
    }

    void OnUpdateSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName);

        connectingView.Close();
        Close();

        if (onChangeDisplayName != null)
        {
            onChangeDisplayName(displayNameInputField.text);
        }
    }

    void OnUpdateFailure(PlayFabError error)
    {
        var report = error.GenerateErrorReport();
        Debug.LogError(report);

        connectingView.Close();
        ErrorDialogView.Show("UpdateUserTitleDisplayName failed", report, OnClickOk, true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
