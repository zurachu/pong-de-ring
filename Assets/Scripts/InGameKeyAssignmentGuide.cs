using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameKeyAssignmentGuide : MonoBehaviour
{
    static readonly int tapIndex = -1;

    [SerializeField] Text tap;
    [SerializeField] Text key;
    [SerializeField] Image keyImage;

    List<char> characters;
    int index;

    // Start is called before the first frame update
    void Start()
    {
        tap.DOFade(0f, 0f);
        key.DOFade(0f, 0f);
        keyImage.DOFade(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        keyImage.DOFade(key.color.a, 0f);
    }

    public void Initialize(List<InGameKeyAssignment> keyAssignments, int index, Vector2 localPosition)
    {
        transform.localPosition = localPosition;

        characters = new List<char>();
        if (keyAssignments != null)
        {
            foreach (var keyAssignment in keyAssignments)
            {
                characters.Add(keyAssignment.Character(index));
            }
        }

        index = tapIndex;
        Show();
    }

    void Show()
    {
        if (index == tapIndex)
        {
            Fade(tap, Show);
        }
        else
        {
            key.text = new string(characters[index], 1);
            Fade(key, Show);
        }

        index++;
        if (index >= characters.Count)
        {
            index = tapIndex;
        }
    }

    void Fade(Text text, TweenCallback onComplete)
    {
        var seq = DOTween.Sequence();
        seq.Append(text.DOFade(0f, 0f));
        seq.Append(text.DOFade(0.5f, 0.5f));
        seq.AppendInterval(1f);
        seq.Append(text.DOFade(0f, 0.5f));
        seq.AppendInterval(1f);
        seq.onComplete = Show;
        seq.Play();
    }
}
