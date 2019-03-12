using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameKeyAssignment : MonoBehaviour
{
    [Serializable]
    public class KeyInfo
    {
        public KeyCode keyCode;
        public char character;
    }

    [SerializeField] List<KeyInfo> keyInfos;

    public KeyCode Code(int index)
    {
        if (!IsValidIndex(index))
        {
            return KeyCode.None;
        }

        return keyInfos[index].keyCode;
    }

    public char Character(int index)
    {
        if (!IsValidIndex(index))
        {
            return '\0';
        }

        return keyInfos[index].character;
    }

    bool IsValidIndex(int index)
    {
        return keyInfos != null
            && index >= 0
            && index < keyInfos.Count;
    }
}
