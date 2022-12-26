using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialButton : MonoBehaviour
{
    public TalkManager talkManager;

    public void SelectButton(int num)
    {
        talkManager.SelectButton(num);
    }
}
