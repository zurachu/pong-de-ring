using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConnectingView : MonoBehaviour
{
    public static GameObject Prefab
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/Common/ConnectingView");
        }
    }

    public static ConnectingView Show()
    {
        return Instantiate(Prefab).GetComponent<ConnectingView>();
    }

    [SerializeField] Text dotText;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("Progress");
    }

    IEnumerator Progress()
    {
        while (true)
        {
            for (var count = 0; count <= 3; count++)
            {
                dotText.text = new string('.', count);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
