using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameKeyAssignment : MonoBehaviour
{
    static readonly Dictionary<KeyCode, KeyCode> keypadNumberKeyMap = new Dictionary<KeyCode, KeyCode> {
        { KeyCode.Keypad0, KeyCode.Alpha0 },
        { KeyCode.Keypad1, KeyCode.Alpha1 },
        { KeyCode.Keypad2, KeyCode.Alpha2 },
        { KeyCode.Keypad3, KeyCode.Alpha3 },
        { KeyCode.Keypad4, KeyCode.Alpha4 },
        { KeyCode.Keypad5, KeyCode.Alpha5 },
        { KeyCode.Keypad6, KeyCode.Alpha6 },
        { KeyCode.Keypad7, KeyCode.Alpha7 },
        { KeyCode.Keypad8, KeyCode.Alpha8 },
        { KeyCode.Keypad9, KeyCode.Alpha9 },
    };

    [Serializable]
    public class KeyInfo
    {
        public KeyCode keyCode;
        public char character;
    }

    [SerializeField] List<KeyInfo> keyInfos;

    public bool GetKeyUp(int index)
    {
        if (!IsValidIndex(index))
        {
            return false;
        }

        var code = Code(index);
        return Input.GetKeyUp(code)
            || (keypadNumberKeyMap.ContainsKey(code) && Input.GetKeyUp(keypadNumberKeyMap[code]));
    }

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
