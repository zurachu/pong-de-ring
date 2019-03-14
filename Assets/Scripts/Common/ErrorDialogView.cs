using System;
using UnityEngine;
using UnityEngine.UI;

public class ErrorDialogView : MonoBehaviour
{
    public static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/Common/ErrorDialogView");
        }
    }

    public static ErrorDialogView Show(string title, string message, Action onClickRetry, bool cancelable = false)
    {
        var view = Instantiate(Prefab).GetComponent<ErrorDialogView>();
        view.Initialize(title, message, onClickRetry, cancelable);
        return view;
    }

    [SerializeField] Text titleText;
    [SerializeField] Text messageText;
    [SerializeField] Button retryButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button closeButton;

    public void Initialize(string title, string message, Action onClickRetry, bool cancelable = false)
    {
        titleText.text = title;
        messageText.text = message;
        retryButton.onClick.AddListener(() => {
            Close();
            onClickRetry();
        });

        if (cancelable)
        {
            cancelButton.onClick.AddListener(Close);
            closeButton.onClick.AddListener(Close);
        }
        else
        {
            var retryButtonPosition = retryButton.transform.localPosition;
            retryButtonPosition.x = 0f;
            retryButton.transform.localPosition = retryButtonPosition;
            cancelButton.gameObject.SetActive(false);
            closeButton.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
