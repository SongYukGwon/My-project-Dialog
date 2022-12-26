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

    //선택지 UI
    public GameObject selectPanel;
    public List<GameObject> buttons;

    //대화 UI
    public GameObject talkPanel;        //대화 패널 UI
    public TextMeshProUGUI talkText;    //대화 내용
    public TextMeshProUGUI nameText;    //npc 이름 

    //대화 변수
    public bool isAction;               //현재 상태
    public bool isSelect;
    public int branch;                  //대화 분기
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

        //버튼 비활성화
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

        //등록된 NPC의 대화 및 선택 정보 받아오기

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
        if (talkContent == null) //반환된 것이 null이면 더이상 남은 대사가 없으므로 action상태변수를 false로 설정 
        {
            isAction = false;
            talkIndex = 0; //talk인덱스 초기화
            selectIndex = 0;
            branch = 0;
            talkPanel.SetActive(isAction); //대화창 활성화 상태에 따라 대화창 활성화 변경
            return;
        }

        nameText.text = name;
        talkText.text = talkContent;


        //다음 문장을 가져오기 위해 talkData의 인덱스를 늘림
        isAction = true; //대사가 남아있으므로 계속 진행되어야함 
        talkPanel.SetActive(isAction); //대화창 활성화 상태에 따라 대화창 활성화 변경

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

    string GetTalk(int talkIndex) //Object의 id , string배열의 index
    {

        //대화내용이 없으면 return
        if (talkIndex == talkData.Count)
        {
            return null;
        }

        //이벤트가 있는지 확인
        int eventIndex = -1;
        if (selectIndex < selectData.Count)
        {
            eventIndex = selectData[selectIndex].eventIndex;
        }


        if (talkIndex == eventIndex)
        {
            Debug.Log("선택지!");
            isSelect = true;
            SeeSelect();
        }

        return talkData[talkIndex].content; //해당 아이디의 해당하는 대사를 반환 
    }

    //선택지 보이게 하는 함수
    void SeeSelect()
    {
        int eventIndex = selectData[selectIndex].eventIndex;

        eventMax = 0;

        //현재 최대 선택지만큼 확인
        for (int i = 0; i < 3; i++)
        {
            if (selectIndex + i < selectData.Count && selectData[selectIndex + i].eventIndex == eventIndex)
                eventMax++;
            else
                break;
        }

        //select UI 활성화
        selectPanel.SetActive(true);
        
        for (int i = 0; i < eventMax; i++)
        {   
            buttons[i].SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = i.ToString() + "." + selectData[selectIndex + i].content;
        }

    }


    //버튼 클릭하였을때
    public void SelectButton(int num)
    {
        //select UI 비활성화
        selectPanel.SetActive(false);

        for (int i = 0; i < 3; i++)
        {
            buttons[i].SetActive(false);
        }


        // 선택지에 따른 분기 설정
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
