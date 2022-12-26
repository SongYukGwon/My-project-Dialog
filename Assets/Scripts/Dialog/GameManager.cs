using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public GameObject scanObject;       //�νĵ� ��ü
    public TalkManager talkManager;     //��ȭ ���� ������

    private void Awake()
    {

    }

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        if(scanObj.GetComponent<NPC>() != null ) {
            NPC objData = scanObject.GetComponent<NPC>();
            talkManager.Talk(objData.id, objData.isNPC, objData.NPCName);
        }   
    }

    
}
