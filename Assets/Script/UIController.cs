using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public GameObject allScript;
    public SaveData saveData;



//공정별 레시피를 제작을 위한 패널
    public Panel_Holder panel_Holder;
    public Panel_Pin panel_Pin;
    public Panel_Cell panel_Cell;
    public Panel_Nickel panel_Nickel;
    public Panel_Welding Panel_Welding;

    //홀더 개수를 표시하는 텍스트들
    public Text[] HolderCountText;
    public Text PinCountText;
    public Text[] CellCountText;
    public Text[] NickelCountText;
    public Text[] WeldingProcessText;
    //홀더가 배치될 때 배치정보를 표시하는 기록 라인
    public GameObject RecordLine;
    //기록라인이 표시되는 Content오브젝트


    public GameObject[] ScrollView;
    public GameObject[] MaterialsInfo;
    public GameObject Content;
    public GameObject Material;

    public GameObject HolderPickButton;
    public GameObject PinAutoSetButton;
    public GameObject CellPickButton;
    public GameObject NickelPickButton;
    public GameObject WeldingStatus;
    public InputField[] WeldingStatusInput = new InputField[4];


    //드롭다운으로 이벤트 발생을 위해 오브젝트를 저장(컴포넌트에서는 왜인지 실행이 안됨)
    public TMP_Dropdown dropdown;
    //홀더 배치가 올바르지 않을 때 표시되는 메시지 
    public GameObject warnningMsg;
    //패널 이동 가능 여부를 결정하는 버튼
    public GameObject MoveableBtn;

    public float[] WeldingValue = new float[4];



    private void Start(){
        dropdown.onValueChanged.AddListener(OnDropDownEvent);
        saveData = allScript.GetComponent<SaveData>();

        panel_Holder = allScript.GetComponent<AllScript>().GetPanel_Holder();
        panel_Pin = allScript.GetComponent<AllScript>().GetPanel_Pin();
        panel_Cell = allScript.GetComponent<AllScript>().GetPanel_Cell();
        panel_Nickel = allScript.GetComponent<AllScript>().GetPanel_Nickel();
    }

    private void Update() {
        allScript.GetComponent<ChangeMode>().ScreenMode = allScript.GetComponent<ChangeMode>().ScreenMode;
    }
    /**********************************************************************************************/
    //버튼 이벤트

    //2X1 버튼 클릭 시
    public void OnClickTwo(){
        Debug.Log("2X1 선택됨");
        panel_Holder.HolderType = Panel_Holder.Type.Two;
    }

    //1X2 버튼 클릭 시
    public void OnClickTwoVertical(){
        Debug.Log("1X2 선택됨");
        panel_Holder.HolderType = Panel_Holder.Type.TwoVertical;
    }

    //3X1 버튼 클릭 시
    public void OnClickThree(){
        Debug.Log("3X1 선택됨");
        panel_Holder.HolderType = Panel_Holder.Type.Three;
    }

    //1X3 버튼 클릭 시
    public void OnClickThreeVertical(){
        Debug.Log("1X3 선택됨");
        panel_Holder.HolderType = Panel_Holder.Type.ThreeVertical;
    }

    //5X5 버튼 클릭 시
    public void OnClickFive(){
        Debug.Log("5X5 선택됨");
        panel_Holder.HolderType = Panel_Holder.Type.Five;
    }
    /***********************************/
    public void OnClickAutoSet(){
        panel_Pin.AutoSet();
    }
    /***********************************/
    public void OnClickPlus50E(){
        Debug.Log("50E+ 선택됨");
        panel_Cell.CellType = Panel_Cell.Type.plus50E;
    }
    public void OnClickMinus50E(){
        Debug.Log("50E- 선택됨");
        panel_Cell.CellType = Panel_Cell.Type.minus50E;
    }
    public void OnClickPlusM50(){
        Debug.Log("M50+ 선택됨");
        panel_Cell.CellType = Panel_Cell.Type.plusM50;
    }
    public void OnClickMinusM50(){
        Debug.Log("M50- 선택됨");
        panel_Cell.CellType = Panel_Cell.Type.minusM50;
    }
    public void OnClickPlus40T(){
        Debug.Log("40T+ 선택됨");
        panel_Cell.CellType = Panel_Cell.Type.plus40T;
    }
    public void OnClickMinus40T(){
        Debug.Log("40T- 선택됨");
        panel_Cell.CellType = Panel_Cell.Type.minus40T;
    }
    /***********************************/
    public void OnClickNickelOne(){
        Debug.Log("니켈 1열 선택됨");
        panel_Nickel.NickelType = Panel_Nickel.Type.Line1;
    }
    public void OnClickNickelTwo(){
        Debug.Log("니켈 2열 선택됨");
        panel_Nickel.NickelType = Panel_Nickel.Type.Line2;
    }
    public void OnClickNickelThree(){
        Debug.Log("니켈 3열 선택됨");
        panel_Nickel.NickelType = Panel_Nickel.Type.Line3;
    }
    public void OnClickNickelFour(){
        Debug.Log("니켈 4열 선택됨");
        panel_Nickel.NickelType = Panel_Nickel.Type.Line4;
    }
    /***********************************/
    public void OnClickMoveable(){
        if(allScript.GetComponent<AllScript>().GetMouseListener().Movealbe){
            allScript.GetComponent<AllScript>().GetMouseListener().Movealbe = false;
            MoveableBtn.transform.GetChild(0).GetComponent<Text>().text = "이동 불가능";
        }else{
            allScript.GetComponent<AllScript>().GetMouseListener().Movealbe = true;
            Debug.Log(MoveableBtn.transform.GetChild(0).name);
            MoveableBtn.transform.GetChild(0).GetComponent<Text>().text = "이동 가능";
        }
        
    }
    
/******************************************************************************/
//기록 라인 변경 이벤트

    public void ChangeIndexClick_Holder(){
        OnClickUpAndDown(ChangeMode.ModeOption.Holder);
    }
    public void ChangeIndexClick_Pin(){
        OnClickUpAndDown(ChangeMode.ModeOption.HolderPin);
    }
    public void ChangeIndexClick_Cell(){
        OnClickUpAndDown(ChangeMode.ModeOption.Cell);
    }
    public void ChangeIndexClick_Nickel(){
        OnClickUpAndDown(ChangeMode.ModeOption.Nickel);
    }
    public void ChangeIndexClick_Welding(){
        OnClickUpAndDown(ChangeMode.ModeOption.Welding);
    }
//기록라인 순서 변경
    public void OnClickUpAndDown(ChangeMode.ModeOption modeOption){
        int contentLength = Content.transform.childCount;
        Debug.Log("누른 단계의 최종 길이 : " + contentLength);
        //클릭된 UI 오브젝트 가져오기
        GameObject gameObject = EventSystem.current.currentSelectedGameObject;

        //가져온 오브젝트의 '부모오브젝트'가 몇번째 자식 오브젝트인지 판단
        int index = gameObject.transform.parent.GetSiblingIndex();
        Debug.Log("누른 라인의 순번 : " + index);
        //바꾸고자 하는 순번
        int changeIndex = index;
        //앞으로 당길지 뒤로 밀지
        int changeType = -1;
        
        bool nextStep = false;

        if(gameObject.name == "Up" && index != 0){    //앞으로 당기기
            changeIndex--;
            changeType = 0;
            nextStep = true;
        }else if(gameObject.name == "Down" && index < contentLength - 1){   //뒤로 밀기
            changeIndex++;
            changeType = 1;
            nextStep = true;   
        }

        if(nextStep){
            switch(modeOption){
                case ChangeMode.ModeOption.Holder:
                    saveData.ChangeProcessHolder(index, changeType);
                break;
                case ChangeMode.ModeOption.HolderPin:
                    saveData.ChangeProcessPin(index, changeType);
                break;
                case ChangeMode.ModeOption.Cell:
                    saveData.ChangeProcessCell(index, changeType);
                break;
                case ChangeMode.ModeOption.Nickel:
                    saveData.ChangeProcessNickel(index, changeType);
                break;
                case ChangeMode.ModeOption.Welding:

                break;
                
            }
        }
        //UI에서 위치 바꾸기
        gameObject.transform.parent.SetSiblingIndex(changeIndex);

        //기록 라인의 순번 표기를 새로고침
        RefreshRecordLineNum();
    }

/**********************************************************************************************/
//프리뷰 플레이 버튼 함수

    public void OnClickPaly(){
        Debug.Log("플레이버튼 클릭됨");
        switch(allScript.GetComponent<ChangeMode>().ScreenMode){
            case ChangeMode.ModeOption.Holder:
                saveData.AllInactiveHolder();
            break;
            case ChangeMode.ModeOption.HolderPin:
                saveData.AllInactivePin();
            break;
            case ChangeMode.ModeOption.Cell:
                saveData.AllInactiveCell();
            break;
            case ChangeMode.ModeOption.Nickel:
                saveData.AllInactiveNickel();
            break;
            case ChangeMode.ModeOption.Welding:
                saveData.AllInactiveWelding();
            break;
        }
    }

/**************************************************************************************************************/
//이하 메서드

//스크롤 뷰 셋팅
    public void SetScrollView(int index){
        Debug.Log(ScrollView[index]);
        if(index < ScrollView.Length){
            for(int i = 0 ; i < ScrollView.Length ; i++){
                if(i == index){
                    ScrollView[i].gameObject.SetActive(true);
                }else{
                    ScrollView[i].gameObject.SetActive(false);
                }
            }
        }
        Content = ScrollView[index].transform.GetChild(0).transform.GetChild(0).gameObject;
        Debug.Log(Content);
    }

//부품 개수 창 셋팅
    public void SetMaterialInfoActive(int index){
        if(index < MaterialsInfo.Length){
            for(int i = 0 ; i < MaterialsInfo.Length ; i++){
                if(i == index){
                    MaterialsInfo[i].gameObject.SetActive(true);
                    Material = MaterialsInfo[i];
                }else{
                    MaterialsInfo[i].gameObject.SetActive(false);
                }
            }
        }
    }

//최초 시작때 한 번 실행되고 이후 홀더 생성, 삭제가 일어날 때도 호출 됨
//홀더 개수 변경시 호출
    public void SetHolderMaterialInfo(){
        int[] ints = saveData.GetHolderCount();
        for(int i = 0 ; i < HolderCountText.Length; i++){
            HolderCountText[i].text = "" + ints[i];
        }
    }

//핀 개수 변경시 호출
    public void SetPinMaterialInfo(){
        PinCountText.text = saveData.GetPinProcessCount() + "";
    }
//셀 개수 변경시 호출
    public void SetCellMaterialInfo(){
        int[] ints = saveData.GetCellCount();
        for(int i = 0 ; i < CellCountText.Length ; i++){
            CellCountText[i].text = "" + ints[i];
        }
    }
//니켈 개수 변경시 호출
    public void SetNickelMaterialInfo(){
        int[] ints = saveData.GetNickelCount();
        for(int i = 0 ; i < NickelCountText.Length ; i++){
            NickelCountText[i].text = "" + ints[i];
        }
    }

    public void SetWeldingProcessInfo(WeldingPixel _weldingPixel){
        WeldingProcessText[0].text = "" + _weldingPixel.Point;
        WeldingProcessText[1].text = "" + _weldingPixel.Time;
        WeldingProcessText[2].text = "" + _weldingPixel.Current;
        WeldingProcessText[3].text = "" + _weldingPixel.Press;
    }
/******************************************************************************/

    //'홀더' 기록라인 생성
    public GameObject AddRecordLine(int num, Panel_Holder.Type holderType, Vector3 pos){

        //기록 라인을 생성하고 line 함수에 저장
        var line = Instantiate(RecordLine, RecordLine.transform.position, Quaternion.identity);

        //line을 '홀더의 스크롤뷰'아래 Content라는 이름의 오브젝트를 찾고 자식 오브젝트로 지정 
        line.transform.SetParent(ScrollView[0].transform.GetChild(0).transform.Find("Content").transform);

        String type = "";

        //배치된 홀더의 좌표
        int x = (int)pos.x;
        int y = (int)pos.y;

        //놓인 홀더의 종류에 맞춰 텍스트를 저장
        switch(holderType){
            case Panel_Holder.Type.Two:
                type = "2X1";
            break;
            case Panel_Holder.Type.TwoVertical:
                type = "1X2";
            break;
            case Panel_Holder.Type.Three:
                type = "3X1";
            break;
            case Panel_Holder.Type.ThreeVertical:
                type = "1X3";
            break;
            case Panel_Holder.Type.Five:
                type = "5X5";
            break;
        }

        //기록라인의 첫번째 자식오브젝트(== 순번)설정
        line.GetComponent<RecordLine>().SetNumText(num.ToString());
        //기록라인의 두번째 자식오브젝트(== 홀더종류)설정
        line.GetComponent<RecordLine>().SetTypeText(type);
        //기록라인의 세번째 자식오브젝트(== 홀더 위치 좌표)설정
        line.GetComponent<RecordLine>().SetCdnText(x + " , " + y);
        Debug.Log(line.transform.GetChild(3).transform.GetChild(0).gameObject.name);
        //위로 아래로 버튼에 함수 할당
        line.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Holder);
        line.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Holder);
        //기록라인의 스케일 설정(이거 안하면 왜인지 모르겠는데 0.8로 설정됨)
        line.transform.localScale = new Vector3(1, 1, 1);

        //생성된 기록라인을 반환
        return line;
    }

/*****************/

    //'핀' 기록라인 생성
    public GameObject AddRecordLinePin(int num, Vector3 pos){
        //기록 라인을 생성하고 line 함수에 저장
        var line = Instantiate(RecordLine, RecordLine.transform.position, Quaternion.identity);
        //line을 '핀의 스크롤뷰'아래 Content라는 이름의 오브젝트를 찾고 자식 오브젝트로 지정 
        line.transform.SetParent(ScrollView[1].transform.GetChild(0).transform.Find("Content").transform);

        //배치된 홀더의 좌표
        int x = (int)pos.x;
        int y = (int)pos.y;

        line.GetComponent<RecordLine>().SetNumText(num.ToString());
        line.GetComponent<RecordLine>().SetTypeText("Pin");
        line.GetComponent<RecordLine>().SetCdnText(x + " , " + y);
        //여기서부터 
        line.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Pin);
        line.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Pin);
        //여기까지
        line.transform.localScale = new Vector3(1,1,1);

        return line;
    }
/*****************/

    //'셀' 기록라인 생성
    public GameObject AddRecordLineCell(int num, Panel_Cell.Type cellType,Vector3 pos){
        //기록 라인을 생성하고 line 함수에 저장
        var line = Instantiate(RecordLine, RecordLine.transform.position, Quaternion.identity);
        //line을 '핀의 스크롤뷰'아래 Content라는 이름의 오브젝트를 찾고 자식 오브젝트로 지정 
        line.transform.SetParent(ScrollView[2].transform.GetChild(0).transform.Find("Content").transform);

        //배치된 홀더의 좌표
        int x = (int)pos.x;
        int y = (int)pos.y;

        String type = "";

        //놓인 홀더의 종류에 맞춰 텍스트를 저장
        switch(cellType){
            case Panel_Cell.Type.plus50E:
                type = "50E+";
            break;
            case Panel_Cell.Type.minus50E:
                type = "50E-";
            break;
            case Panel_Cell.Type.plusM50:
                type = "M50+";
            break;
            case Panel_Cell.Type.minusM50:
                type = "M50-";
            break;
            case Panel_Cell.Type.plus40T:
                type = "40T+";
            break;
            case Panel_Cell.Type.minus40T:
                type = "40T-";
            break;
        }

        line.GetComponent<RecordLine>().SetNumText(num.ToString());
        line.GetComponent<RecordLine>().SetTypeText(type);
        line.GetComponent<RecordLine>().SetCdnText(x + " , " + y);
        //여기서부터 
        line.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Cell);
        line.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Cell);
        //여기까지
        line.transform.localScale = new Vector3(1,1,1);


        return line;
    }
/*****************/

    //'니켈' 기록라인 생성
    public GameObject AddRecordLineNickel(int _num, Panel_Nickel.Type _nickelType, int[,] _startAndLast){
         var line = Instantiate(RecordLine, RecordLine.transform.position, Quaternion.identity);
         line.transform.SetParent(ScrollView[3].transform.GetChild(0).transform.Find("Content").transform);

        String type;

        switch(_nickelType){
            case Panel_Nickel.Type.Line1:   
                type = "1열";
            break;
            case Panel_Nickel.Type.Line2:
                type = "2열";
            break;
            case Panel_Nickel.Type.Line3:
                type = "3열";
            break;
            case Panel_Nickel.Type.Line4:
                type = "4열";
            break;
            default:
                type = "default";
            break;
        }

        line.GetComponent<RecordLine>().SetNumText(_num.ToString());
        line.GetComponent<RecordLine>().SetTypeText(type);
        line.GetComponent<RecordLine>().SetCdnText("(" + _startAndLast[0,0] + "," + _startAndLast[0,1] + ") / (" + _startAndLast[1,0] + "," + _startAndLast[1,1] + ")");

        line.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Nickel);
        line.transform.GetChild(4).gameObject.GetComponent<Button>().onClick.AddListener(ChangeIndexClick_Nickel);

        line.transform.localScale = new Vector3(1,1,1);
        return line;
    }

    //'용접' 기록라인 생성
    public GameObject AddRecordLineWelding(int _num, WeldingPixel _weldingPixel){ 
        var line = Instantiate(RecordLine, RecordLine.transform.position, Quaternion.identity);
        line.transform.SetParent(ScrollView[4].transform.GetChild(0).transform.Find("Content").transform);

        String weldingValue = _weldingPixel.Point + ", " + _weldingPixel.Time + ", " + _weldingPixel.Current + ", " + _weldingPixel.Press;
        
        line.GetComponent<RecordLine>().SetNumText(_num.ToString());
        line.GetComponent<RecordLine>().SetTypeText(weldingValue);
        line.GetComponent<RecordLine>().SetCdnText("(" + _weldingPixel.gameObject.transform.position.x + "," + _weldingPixel.gameObject.transform.position.y + ")");
    
        //용접 프로세스는 순서변경을 하지 않으므로 화살표 버튼을 제거한다.
        line.transform.GetChild(3).gameObject.SetActive(false);
        line.transform.GetChild(4).gameObject.SetActive(false);

        line.transform.localScale = new Vector3(1,1,1);

        return line;
    }

    public void RemoveRecordLineForWelding(){
        int index = ScrollView[4].transform.GetChild(0).transform.Find("Content").transform.childCount;
        for(int i = 0 ; i < index ; i++){
            Destroy(ScrollView[4].transform.GetChild(0).transform.Find("Content").transform.GetChild(i).gameObject);
        }
    }

/********************************************************************/

    //공정순서 순번 새로고침
    public void RefreshRecordLineNum(){
        int count = Content.transform.childCount;
        for(int i = 0; i < count; i++){
            RecordLine line = Content.transform.GetChild(i).GetComponent<RecordLine>();
            line.SetNumText((i+1).ToString());
        }
    }

    //홀더배치에 따른 다음단계 UI 활성화/비활성화
    public void InteractableSetDropdown(){
        if(panel_Holder.isSquare){
            dropdown.interactable = true;
            warnningMsg.SetActive(false);
        }else{
            dropdown.interactable = false;
            warnningMsg.SetActive(true);
        }
    }

    //드롭다운으로 이벤트 발생
    public void OnDropDownEvent(int index){
        Debug.Log(index);
        //모드변경
        allScript.GetComponent<ChangeMode>().ChangeToMode(index);
    }

/**********************************************************************/

    public void OnInputField(int _index){
        if(_index >= 0 && _index < WeldingStatusInput.Length){
            WeldingValue[_index] = float.Parse(WeldingStatusInput[_index].text);
        }
    }
}
