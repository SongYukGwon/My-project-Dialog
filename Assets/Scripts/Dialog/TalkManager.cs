using JetBrains.Annotations;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct eventSelect
{
    public int eventIndex;
    public string content;
    public int branch;
    public int nextIndex;
}

public struct talkStu
{
    public string content;
    public int branch;
}






public class TalkManager : MonoBehaviour
{
    private List<talkStu> talkData;
    private List<eventSelect> selectData;

    [Serialize]
    public List<int> ids;
    [Serialize]
    public List<DB> dialogs;

    //������ UI
    public GameObject selectPanel;
    public List<GameObject> buttons;

    //��ȭ UI
    public GameObject talkPanel;        //��ȭ �г� UI
    public TextMeshProUGUI talkText;    //��ȭ ����
    public TextMeshProUGUI nameText;    //npc �̸� 

    //��ȭ ����
    public bool isAction;               //���� ����
    public bool isSelect;
    public int branch;                  //��ȭ �б�
    public int talkIndex;
    public int selectIndex;
    public int eventMax;



    // Start is called before the first frame update
    void Awake()
    {
        talkData = new List<talkStu>();
        selectData = new List<eventSelect>();
        isAction = false;
        isSelect = false;
        branch = 0;

        //��ư ��Ȱ��ȭ
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    void GenerateData(int id)
    {
        talkIndex = 0;
        selectIndex = 0;


        int dbId = ids.IndexOf(id);
        talkData.Clear();
        selectData.Clear();

        Debug.Log(dbId);

        //��ϵ� NPC�� ��ȭ �� ���� ���� �޾ƿ���

        for (int j = 0; j < dialogs[dbId].Talk.Count; j++)
        {
            talkData.Add(new talkStu
            {
                content = dialogs[dbId].Talk[j].content,
                branch = dialogs[dbId].Talk[j].branch
            });
        }
        for (int j = 0; j < dialogs[dbId].Select.Count; j++)
        {
            selectData.Add(new eventSelect
            {
                eventIndex = dialogs[dbId].Select[j].eventIndex,
                content = dialogs[dbId].Select[j].content,
                nextIndex = dialogs[dbId].Select[j].index,
                branch = dialogs[dbId].Select[j].branch
            });
        }

    }

    public void Talk(int id, bool isNPC, string name)
    {
        if (isAction == false)
        {
            GenerateData(id);
        }

        string talkContent = GetTalk(talkIndex);
        if (talkContent == null) //��ȯ�� ���� null�̸� ���̻� ���� ��簡 �����Ƿ� action���º����� false�� ���� 
        {
            isAction = false;
            talkIndex = 0; //talk�ε��� �ʱ�ȭ
            selectIndex = 0;
            branch = 0;
            talkPanel.SetActive(isAction); //��ȭâ Ȱ��ȭ ���¿� ���� ��ȭâ Ȱ��ȭ ����
            return;
        }

        nameText.text = name;
        talkText.text = talkContent;


        //���� ������ �������� ���� talkData�� �ε����� �ø�
        isAction = true; //��簡 ���������Ƿ� ��� ����Ǿ���� 
        talkPanel.SetActive(isAction); //��ȭâ Ȱ��ȭ ���¿� ���� ��ȭâ Ȱ��ȭ ����

        if (!isSelect)
        {
            talkIndex++;
            while (talkIndex != talkData.Count && talkData[talkIndex].branch != 0 && talkData[talkIndex].branch != branch)
            {
                Debug.Log(talkIndex);
                talkIndex++;
            }
        }
    }

    string GetTalk(int talkIndex) //Object�� id , string�迭�� index
    {

        //��ȭ������ ������ return
        if (talkIndex == talkData.Count)
        {
            return null;
        }

        //�̺�Ʈ�� �ִ��� Ȯ��
        int eventIndex = -1;
        if (selectIndex < selectData.Count)
        {
            eventIndex = selectData[selectIndex].eventIndex;
        }


        if (talkIndex == eventIndex)
        {
            Debug.Log("������!");
            isSelect = true;
            SeeSelect();
        }

        return talkData[talkIndex].content; //�ش� ���̵��� �ش��ϴ� ��縦 ��ȯ 
    }

    //������ ���̰� �ϴ� �Լ�
    void SeeSelect()
    {
        int eventIndex = selectData[selectIndex].eventIndex;

        eventMax = 0;

        //���� �ִ� ��������ŭ Ȯ��
        for (int i = 0; i < 3; i++)
        {
            if (selectIndex + i < selectData.Count && selectData[selectIndex + i].eventIndex == eventIndex)
                eventMax++;
            else
                break;
        }

        //select UI Ȱ��ȭ
        selectPanel.SetActive(true);
        
        for (int i = 0; i < eventMax; i++)
        {   
            buttons[i].SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString() + "." + selectData[selectIndex + i].content;
        }

    }


    //��ư Ŭ���Ͽ�����
    public void SelectButton(int num)
    {
        //select UI ��Ȱ��ȭ
        selectPanel.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            buttons[i].SetActive(false);
        }


        // �������� ���� �б� ����
        int nextIndex = selectData[selectIndex + num].nextIndex;
        branch = selectData[selectIndex + num].branch;
        selectIndex += eventMax;

        isSelect = false;

        talkIndex = nextIndex;

        Talk(1, true, nameText.text);

        Debug.Log(num);
        return;
    }

    


}
