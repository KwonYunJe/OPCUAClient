using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Panel_Holder : MonoBehaviour
{
    public GameObject AllScript;
    public SaveData saveData;
    public UIController UIController;



    //홀더를 종류별로 담는 배열
    public GameObject[] Holder;

    //좌표 선택 가능 불가를 결정하는 floag
    public bool CanSelectPanel;

    //현재 마우스가 호버링 중인 픽셀의 좌표를 담을 배열 == 마우스가 올라간 픽셀의 좌표
    public GameObject nowPixel;
    //마우스가 호버링 중일 때 주변 좌표값을 담을 리스트 == 홀더가 포함할 모든 좌표
    public List<int[]> SurroundAllCrd;
    //마우스가 호버링 중인 좌표의 오브젝트를 담을 리스트 ==홀더가 포함할 모든 좌표의 Pixel 오브젝트
    public List<GameObject> returnList;

    List<GameObject> selectedObj;
    //
    public int[] minCrd = new int[]{0,0};
    public int[] maxCrd = new int[]{0,0};




    //중요!!! 좌표가 (x,y)라면 배열에는 (y,x)로 저장됨!!
    public GameObject[,] pixelCount = new GameObject[12,16] ;

    //홀더타입 
    public enum Type { Two, TwoVertical, Three, ThreeVertical, Five } ;
    public Type HolderType;

    //홀더의 배치가 사각형인지 나타내는 boolean
    public bool isSquare;


    private void Start() {
        //row별 오브젝트를 2차원 배열에 담음
        startPutArray();            
        //saveData 스크립트를 AllScript에서 가져옴
        saveData = AllScript.GetComponent<SaveData>();
        UIController = AllScript.GetComponent<AllScript>().GetUIController();
    }

    private void Update() {
    }

    //row별로 담긴 좌표를 2차원 배열에 담는다.
    private void startPutArray(){
        for(int i = 0 ; i < 12 ; i++){
            for(int j= 0 ; j < 16 ; j++){
                pixelCount[i,j] = gameObject.transform.GetChild(i).transform.GetChild(j).gameObject;
            }
        } 
    }


/**********************************************************************/

    //1. 좌표가 마우스 감지되었을 때 가장 먼저 발동되는 함수
    public void pickedPixel(GameObject obj){
        pixelTintingDefault();
        //매개변수로 받은 오브젝트의 좌표를 저장
        nowPixel = obj;
        //감지된 오브젝트의 좌표값을 매개변수로 다음 함수를 실행 
        List<int[]> ints = returnCrd((int)obj.transform.position.x, (int)obj.transform.position.y);
        //Debug.Log("좌표의 개수 : " +ints.Count);


        //좌표값 리스트를 매개변수로 다음 함수를 실행
        selectedObj = returnObjectList(ints);
        //Debug.Log("유효한 좌표의 개수 : " + selectedObj.Count);
        for(int i = 0 ; i < selectedObj.Count ; i++){
            int x = (int)selectedObj[i].transform.position.x;
            int y = (int)selectedObj[i].transform.position.y;
            //Debug.Log("유효한 좌표 " + i + ": " + x + ", " + y);
        }

        //홀더로 선택된 좌표의 개수와 유효한 좌표의 개수가 일치하면
        if(ints.Count == selectedObj.Count){
            pixelTintingGreen(selectedObj);
            CanSelectPanel = true;
        //홀더로 선택된 좌표의 개수와 유효한 좌표의 개수가 일치하지 않으면
        }else{
            pixelTintingRed(selectedObj);
            CanSelectPanel = false;
        }
    }

    //2.
    //선택한 좌표로부터 홀더 타입별로 인근 좌표를 반환하는 메서드 
    //선택한 좌표의 x값, y값, 홀더 타입을 매개변수로 받는다.
    public List<int[]> returnCrd(int x, int y){
        //반환할 좌표값을 담을 리스트
        SurroundAllCrd = new List<int[]> ();

        //홀더타입별로 처리를 달리 함.
        switch(HolderType){
            case Type.Two:
                //2구 홀더의 경우 선택픽셀 + 오른쪽 픽셀을 반환
                for(int i = x ; i <= x + 1 ; i++){
                    SurroundAllCrd.Add(new int[]{i,y});
                }
            break;
            case Type.TwoVertical:
                //2구 세로 홀더의 경우 선택픽셀 + 위쪽 픽셀을 반환
                for(int i = y ; i <= y + 1 ; i++){
                    SurroundAllCrd.Add(new int[]{x,i});
                }
            break;
            case Type.Three:
                //3구 홀더의 경우 선택픽셀 + 왼쪽 + 오른쪽 픽셀을 반환
                for(int i = x - 1 ; i <= x + 1 ; i++){
                    SurroundAllCrd.Add(new int[]{i,y});
                }
            break;
            case Type.ThreeVertical:
                //3구 세로 홀더의 경우 선택픽셀 + 위쪽 + 아래쪽 픽셀을 반환
                for(int i = y - 1 ; i <= y + 1 ; i++){
                    SurroundAllCrd.Add(new int[]{x,i});
                }             
            break;
            case Type.Five:
                //5x5 홀더의 경우 선택픽셀이 가운데가 되도록 좌표를 반환
                for(int i = y - 2 ; i <= y + 2 ; i++){
                    for(int j = x - 2 ; j <= x + 2 ; j++){
                        SurroundAllCrd.Add(new int[]{j,i});
                    }
                }
                
            break;            
        }

        return SurroundAllCrd;
    }

    //3.
    //좌표값을 받아서 유효한 픽셀 오브젝트를 반환하는 메서드, 만약 받은 좌표 중에 오브젝트가 없는 값이 있다면 그냥 추가하지 않고 넘어간다.
    public List<GameObject> returnObjectList(List<int[]> ints){

        //반환할 오브젝트를 담을 리스트
        returnList = new List<GameObject>();

        //매개변수의 리스트 길이만큼 반복
        for(int i = 0 ; i < ints.Count ; i++){
            //탐색에 사용할 x값, y값을 따로 처리
            int x = ints[i][0];
            int y = ints[i][1];

            //만약 x,y값이 유효한 좌표라면 픽셀 배열에서 오브젝트를 찾아 리스트로 추가
            //유효하지 않은 값은 처리되지 않도록 하기 위함
            if(x>=0 && x <=15 && y>=0 && y<=11){
                //픽셀이 존재하더라도 그 픽셀이 다른 홀더에 포함되어 있지 않아야 함.
                //null이 아니라면 이미 다른 홀더에 포함된 픽셀이므로 아래의 내용이 처리되지 않음.
                if(pixelCount[y,x].GetComponent<Pixel>().Holder == null){
                    returnList.Add(pixelCount[y,x]);
                }
            }
        }

        return returnList;
    }

/************************************************************************************/
    //픽셀의 틴트와 리스트에 기록된 좌표를 초기화 하는 함수
    public void initValue(){
        pixelTintingDefault();
        SurroundAllCrd = null;
        returnList = null;
    }

    //홀더를 놓는 함수
    public void SelectPixel(){
        if(CanSelectPanel){
            PutHolder();          //홀더 오브젝트 생성
            //SetHoderCount();      //홀더 개수 재 카운트
        }else{
            Debug.Log("선택할 수 없습니다.");
        }
    }

    //맨 마지막에 생성된 홀더를 제거 == 되돌리기 
    public void Reverse(){
        if(saveData.GetHolderProcessCount() > 0){
            //홀더 제거 메서드 실행
            saveData.DeleteLastIndex_Holder();
            //홀더 개수 재 카운트
            //SetHoderCount();        
            SetMinMax();
        }else{
            Debug.Log("더 이상 되돌릴 수 없습니다.");
        }
        CheckSquare();
    }

/************************************************************************************/
    
    //픽셀을 초록색으로 틴팅
    public void pixelTintingGreen(List<GameObject> objects){
        pixelTintingDefault();
        for(int i = 0 ; i < objects.Count; i++){
            SpriteRenderer spriteRenderer = objects[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 1, 0, 0.5f);
        }
    }
    //픽셀을 빨간색으로 틴팅
    public void pixelTintingRed(List<GameObject> objects){
        pixelTintingDefault();
        for(int i = 0 ; i < objects.Count; i++){
            SpriteRenderer spriteRenderer = objects[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
        }
    }
    //픽셀 틴트를 초기화
    public void pixelTintingDefault(){
        if(selectedObj == null){
            return;
        }
        if(AllScript.GetComponent<ChangeMode>().ScreenMode == ChangeMode.ModeOption.Holder){
            for(int i = 0; i < selectedObj.Count ; i++){
                selectedObj[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }else if(AllScript.GetComponent<ChangeMode>().ScreenMode == ChangeMode.ModeOption.Cell){
            for(int i = 0; i < selectedObj.Count ; i++){
                selectedObj[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.9f);
            }
        }
        
    }
    


    /**************************************************************************************/
    //홀더 오브젝트 생성
    public void PutHolder(){
        //2구 홀더 배치값 조정을 위해 선언한 좌표 값
        Vector3 nowCrd = nowPixel.transform.position;
        Vector3 pos = new Vector3(nowCrd.x, nowCrd.y, 0);

        //생성할 홀더가 담길 변수
        GameObject newHolder = null;

        //현재 설정된 홀더 타입에 따른 처리
        switch(HolderType){
            case Type.Two:
                //2구 홀더 배치 값 조정
                pos = pos + Vector3.right * 0.5f;

                //홀더생성(오브젝트, 위치, 각도)
                newHolder = Instantiate(Holder[0], pos, Holder[0].transform.rotation);
            break;
            case Type.TwoVertical:
                pos = pos + Vector3.up * 0.5f;
                newHolder = Instantiate(Holder[1], pos, Holder[1].transform.rotation);
            break;
            case Type.Three:
                newHolder = Instantiate(Holder[2], pos, Holder[2].transform.rotation);
            break;
            case Type.ThreeVertical:
                newHolder = Instantiate(Holder[3], pos, Holder[3].transform.rotation);
            break;
            case Type.Five:
                newHolder = Instantiate(Holder[4], pos, Holder[4].transform.rotation);
            break;
        }

        //홀더의 최소점 최대점 지정
        newHolder.GetComponent<Holder>().MinCrd = SurroundAllCrd[0];
        newHolder.GetComponent<Holder>().MaxCrd = SurroundAllCrd[SurroundAllCrd.Count - 1];

        //홀더가 놓인 '기록라인' 오브젝트 생성, 생성된 오브젝트를 변수에 저장(메서드가 반환값이 있는 메서드임)
        GameObject RecordLine = 
            UIController.GetComponent<UIController>().AddRecordLine(
                saveData.GetHolderProcessCount()+1 , 
                newHolder.GetComponent<Holder>().HolderType,
                newHolder.transform.position
                );
        //'홀더'오브젝트의 스크립트에 '기록라인'오브젝트를 저장
        newHolder.GetComponent<Holder>().RecordLine = RecordLine ;
        //'홀더'오브젝트의 스크립트에 SetPixel(픽셀에 저장된 정보 수정)메서드를 실행, 매개변수는 홀더 영역만큼 지정된 픽셀 개수(List)
        newHolder.GetComponent<Holder>().SetPixel(returnList);
        //'홀더'오브젝트를 일괄관리하는 saveData 스크립트에 저장하는 메서드에 매개변수로 메서드 실행
        saveData.AddHolderProcess(newHolder.GetComponent<Holder>());
        //최대, 최소점 초기화
        SetMinMax();
        CheckSquare();
    }

/******************************************************************************/
//다른 모드에서 다시 홀더모드로 돌아왔을 때 비활성화된 픽셀들을 다시 활성화 하는 메서드
    public List<GameObject> inActiveOther(){
        List<GameObject> ActivePixel = new();

        for(int i = 0; i  < pixelCount.Length/ pixelCount.GetLength(0); i++){
            for(int j = 0 ; j < pixelCount.GetLength(0) ; j++){
                if(pixelCount[j,i].GetComponent<Pixel>().Holder != null){
                    ActivePixel.Add(pixelCount[j,i]);
                }else{
                    pixelCount[j,i].gameObject.transform.Find("tint").gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.9f);
                }
            }
        }
        return ActivePixel;
    }

    public void activeOther(){
        foreach(GameObject _pixel in pixelCount){
                _pixel.transform.Find("tint").gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0); 
        }
    }
/*****************************************************************************/

    //현재 배치된 홀더의 형태가 사각형의 형태인지 판단하는 메서드
    public void CheckSquare(){
        Debug.Log("최소 좌표 " + minCrd[0] + ", " + minCrd[1]);
        Debug.Log("최대 좌표 " + maxCrd[0] + ", " + maxCrd[1]);
        //홀더가 놓여있는 최대값 최소값을 통해 '사각형일 때 정해지는 픽셀의 개수'를 구한다.
        int xLength = maxCrd[0] - minCrd[0] + 1;
        int yLength = maxCrd[1] - minCrd[1] + 1;

        int needTotal = xLength * yLength;

        //pixel 스크립트의 holder(의 변수가 null인지를 판단하여)를 통해 실제로 배치된 홀더가 차지하는 픽셀 개수를 구한다.
        int realTotal = 0 ;
        foreach(GameObject _pixelCount in pixelCount){
            if(_pixelCount.GetComponent<Pixel>().Holder != null){
                realTotal++;
            }
        }

        Debug.Log("요구 칸 수 : " + needTotal + " / 실제 칸 수 : " + realTotal);

        //만약 두 변수의 값이 동일하면 현재 사각형 모양인것이고
        if(realTotal == needTotal){
            isSquare = true;
        //아니라면 사각형이 아니기에 해당하는 여부를 전역변수에 적용한다.
        }else{
            Debug.Log("홀더가 정상적으로 배치되지 않았습니다.");
            isSquare = false;
        }

        //그리고 그 여부에 따라 다음단계로 넘어갈 수 있는 과정을 활성화/비활성화 한다.
        UIController.InteractableSetDropdown();
    }

    public void SetMinMax(){
        //현재 최소, 최대을 0으로 초기화한다.
        //0으로 초기화하지 않으면 되돌리기를 했을 때 최대 값이 새로고침되지 않는 경우가 있다.
        minCrd = new int[]{0,0};
        maxCrd = new int[]{0,0};
        //홀더가 놓일 때마다 최소점(왼쪽 아래) 최대점(오른쪽 위) 지점을 초기화한다.
        for(int i = 0 ; i < saveData.GetHolderProcessCount() ; i++){
            Debug.Log(saveData.GetHolder(i).MinCrd[0] + "," + saveData.GetHolder(i).MinCrd[1] + " / " + saveData.GetHolder(i).MaxCrd[0] + "," + saveData.GetHolder(i).MaxCrd[1]);
            if(1 == saveData.GetHolderProcessCount() && i == 0){
                //최소 지점은 0번째 탐색중인 요소로 지정한다.
                minCrd = new int[]{saveData.GetHolder(i).MinCrd[0] , saveData.GetHolder(i).MinCrd[1]};
            }

            //아래는 각 최소값보다 작을 때 최소값으로 지정, 최대값보다 클 때 최대값으로 지정 하는 조건문이다.
            if(saveData.GetHolder(i).MinCrd[0] < minCrd[0]){
                Debug.Log("x좌표 최소 감지 됨");
                minCrd[0] = saveData.GetHolder(i).MinCrd[0];
            }
            if(saveData.GetHolder(i).MinCrd[1] < minCrd[1]){
                Debug.Log("y좌표 최소 감지 됨");
                minCrd[1] = saveData.GetHolder(i).MinCrd[1];
            }
            if(saveData.GetHolder(i).MaxCrd[0] > maxCrd[0]){
                Debug.Log("x좌표 최대 감지 됨");
                maxCrd[0] = saveData.GetHolder(i).MaxCrd[0];
            }
            if(saveData.GetHolder(i).MaxCrd[1] > maxCrd[1]){
                Debug.Log("y좌표 최대 감지 됨");
                maxCrd[1] = saveData.GetHolder(i).MaxCrd[1];
            }
        }
    }
}
