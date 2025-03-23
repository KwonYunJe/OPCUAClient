using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    //카메라 움직이기 버튼
    public bool Moveable = false;
    
    public GameObject RecordObj;

    private TMP_Dropdown ModeDropdown;

    public Record record;

    public GameObject[] RecordContent;

    // Start is called before the first frame update
    void Start()
    {
        SetDropdownOptions();
        record = new Record();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //모드변경을 위한 드롭다운 셋팅
    void SetDropdownOptions(){
        ModeDropdown = transform.Find("Body").Find("ProcessInfo").Find("Process").Find("ProcessDropDown").GetComponent<TMP_Dropdown>();
        ModeDropdown.onValueChanged.AddListener(delegate {
            OnClickDropDown(ModeDropdown.value);
        });

        ModeDropdown.options.Clear();
        foreach(string str in Enum.GetNames(typeof(Property.TaskType))){
            ModeDropdown.options.Add(new TMP_Dropdown.OptionData(str));
        }
        
    }

    //카메라 움직이기 버튼 클릭
    public void OnClickCameraMove(){
        if(Moveable){
            Moveable = false;
            transform.Find("Header").Find("MoveButton").Find("SettingBtnText").GetComponent<Text>().text = "이동 불가능";
        }else{
            Moveable = true;
            transform.Find("Header").Find("MoveButton").Find("SettingBtnText").GetComponent<Text>().text = "이동 가능";
        }
    }

    //드롭다운 클릭(모드 변경)
    public void OnClickDropDown(int index){
        Debug.Log("모드 변경 : " + index);
        GameManager.instance.property.SetProperty(index);
    }

    //레코드 라인 생성 및 생성된 오브젝트 반환
    public GameObject CreateRecord(string _num, string _first, string _second){
        GameObject RecordLine = Instantiate(RecordObj, new Vector3(0, 0, 0), Quaternion.identity);
        RecordLine.transform.SetParent(
            RecordContent[(int)GameManager.instance.property.taskType].transform
            // transform.Find("Body").Find("ProcessInfo").Find("Record View")
            // .Find("Scroll View_" + GameManager.instance.property.taskType.ToString())
            // .Find("Viewport").Find("Content")
            );
        
        RecordLine.transform.Find("Num").Find("Text (Legacy)").GetComponent<Text>().text = _num;
        RecordLine.transform.Find("Type").Find("Text (Legacy)").GetComponent<Text>().text = _first;
        RecordLine.transform.Find("Coordinate").Find("Text (Legacy)").GetComponent<Text>().text = _second;

        record.AddRecordLine(RecordLine);
        return RecordLine;
    }

    public void ChangeRecord(){

    }

    public void DeleteRecord(){

    }
}

public class Record{
    List<GameObject> HolderRecord = new List<GameObject>();
    List<GameObject> PinRecord = new List<GameObject>();
    List<GameObject> CellRecord = new List<GameObject>();
    List<GameObject> NickelRecord = new List<GameObject>();
    List<GameObject> WeldingRecord = new List<GameObject>();

    public void AddRecordLine(GameObject gameObject){
        switch(GameManager.instance.property.taskType){
            case Property.TaskType.Holder:
                HolderRecord.Add(gameObject);
                break;
            case Property.TaskType.Pin:
                PinRecord.Add(gameObject);
                break;
            case Property.TaskType.Cell:
                CellRecord.Add(gameObject);
                break;
            case Property.TaskType.Nickel:
                NickelRecord.Add(gameObject);
                break;
            case Property.TaskType.Welding:
                WeldingRecord.Add(gameObject);
                break;
        }
    }
}
