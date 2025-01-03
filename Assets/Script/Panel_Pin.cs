using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Panel_Pin : MonoBehaviour
{
    public GameObject allScript;
    private SaveData saveData;
    public GameObject[,] pixel = new GameObject[11,15];

    //선택된 픽셀
    public GameObject picked;

    void Start()
    {
        saveData = allScript.GetComponent<SaveData>();
        PutArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PutArray(){
        Debug.Log("핀 배열 배치 시작");
        for(int i = 0 ; i < 11 ; i++){
            for(int j = 0 ; j < 15 ; j++){
                pixel[i,j] = gameObject.transform.GetChild(i).transform.GetChild(j).gameObject;
            }

        }
        Debug.Log(pixel[10,14]);
    }

//모드가 전환될 때 핀을 놓을 수 있는 자리만 활성화
    public void ActiveObject(List<GameObject> _activeList){
        //모드가 홀더배치 모드에서 넘어오거나, 그렇지 않다면 핀의 셋팅이 완료되어 있지 않은 경우에 실행
        if((allScript.GetComponent<ChangeMode>().ScreenMode == ChangeMode.ModeOption.Holder)
        ||((allScript.GetComponent<ChangeMode>().ScreenMode != ChangeMode.ModeOption.Holder) && saveData.GetPinProcessCount() == 0)){
            //포지션을 담을 리스트 생성 
            List<Vector3> pos = new List<Vector3>();

            //(활성화된 픽셀의 오브젝트)리스트의 길이만큼 반복
            for(int i = 0 ; i < _activeList.Count ; i++){
                //리스트의 오브젝트중 위치 값만 빼서 이전에 생성한 리스트에 추가
                pos.Add(_activeList[i].transform.position);
            }

            //핀이 놓일 수 있는 모든 좌표를 탐색
            for(int i = 0 ; i < 15; i++){
                for(int j = 0 ; j < 11 ; j++){
                    //핀의 좌표를 일단 저장 
                    float x = pixel[j,i].transform.position.x;
                    float y = pixel[j,i].transform.position.y;

                    //핀의 좌표를 기준으로 좌하단, 좌상단, 우하단, 우상단 좌표를 저장
                    Vector3 a = new Vector3(x,y,0);
                    Vector3 b = new Vector3(x+1,y,0);
                    Vector3 c = new Vector3(x,y+1,0);
                    Vector3 d = new Vector3(x+1,y+1,0);

                    //좌하단, 좌상단, 우하단, 우상단 좌표가 맨 위에서 생성한 리스트에 존재하면 다음 함수를 실행
                    //모두 존재하면 핀을 놓는 곳을 기준으로 네 방향에 모두 홀더가 존재함을 의미
                    if(pos.Contains(a) 
                    && pos.Contains(b)
                    && pos.Contains(c)
                    && pos.Contains(d)){
                        //해당 핀 좌표의 오브젝트 및 시각적 표현을 활성화
                        pixel[j,i].GetComponent<Pin>().SetEnable(true);
                        pixelTintingBlack(pixel[j,i]);
                    }
                }
            }
        }
    }

    //마우스가 핀 픽셀 위에 올라갔을 때 호출되는 함수
    public void pickedPixel(GameObject gameObject){
        //핀 픽셀이 이미 선택된 오브젝트라면 이하 함수를 실행하지 않음
        if(gameObject.GetComponent<Pin>().isPicked == true){
            return;
        }
        //마우스가 감지한 오브젝트를 넘겨받고 전역 변수에 저장
        picked = gameObject;
        //핀 픽셀을 초록색으로 변경
        pixelTintingGreen(picked);
    }

    //마우스가 핀 픽셀 범위를 벗어났을 때 호출되는 함수
    public void unPickedPixel(){
        //전역 변수가 비어있지 않을때만 실행
        if(picked != null){
            //핀 픽셀이 이미 선택된 오브젝트라면 이하 함수를 실행하지 않음
            if(picked.GetComponent<Pin>().isPicked == true){
                return;
            }
            //핀 픽셀을 다시 투명으로 변경
            pixelTintingInactive(picked);
            //전역 변수를 비움
            picked = null;
        }
        
    }

    //모드가 전환되면서 모든 핀 픽셀을 초기화
    public void allInActive(){
        //모든 핀 픽셀을 탐색
        foreach(GameObject _pixel in pixel){
            //색을 원래대로 돌려놓기
            pixelTintingDefault(_pixel);
            //픽셀의 지정 변수 초기화
            _pixel.GetComponent<Pin>().isPicked = false;
            //픽셀을 비활성화
            _pixel.GetComponent<Pin>().SetEnable(false);

        }
        //저장된 공정정보를 초기화하는 메서드 호출
        saveData.ResetProcessPin();
    }

    //마우스가 클릭됐을 때 호출되는 함수 
    public void SelectPixel(){
        //전역변수가 비어있다면 아래 함수는 실행되지 않음
        if(picked == null){
            return;
        }
        //전역변수에 있는 핀 픽셀의 지정여부가 false일때만 함수 작동
        if(picked.GetComponent<Pin>().isPicked == false){
            //선택된 핀 오브젝트의 '선택됨' 플래그를 true로 변경
            GameObject selected = picked;
            selected.GetComponent<Pin>().isPicked = true;
            //그 오브젝트의 tint를 변경
            pixelTintingPick(selected);
            //기록라인 생성 및 반환받기
            selected.GetComponent<Pin>().RecordLine = 
            allScript.GetComponent<AllScript>().GetUIController().AddRecordLinePin(
                saveData.GetPinProcessCount()+1,
                selected.transform.position
            );


            //위 과정을 모두 거친 핀을 saveData에 저장
            saveData.AddPinProcess(selected.GetComponent<Pin>());

            initValue();
        }
    }

    //전역변수를 비움
    public void initValue(){
        picked = null;
    }

    //마우스가 우클릭되었을 때 호출되는 함수
    public void Reverse(){
        //저장된 공정정보가 한개 이상 존재할 때만 실행됨
        if(saveData.GetPinProcessCount() > 0){
            //공정정보의 맨 뒷 정보를 삭제하는 메서드 호출
            saveData.DeleteLastIndex_Pin();
        }else{
            Debug.Log("더 이상 되돌릴 수 없습니다.");
        }
    }

    //자동으로 활성화 된 핀 픽셀을 모두 공정정보에 담는 메서드 
    //버튼 클릭을 통해 호출됨
    public void AutoSet(){
        foreach(GameObject _pixel in pixel){
            if(_pixel.activeSelf){
                picked = _pixel;
                SelectPixel();
            }
        }
    }
    



/************************************************************************************/

    public void pixelTintingDefault(GameObject gameObject){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        gameObject.transform.Find("center").GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
        gameObject.transform.Find("background").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }
    public void pixelTintingPick(GameObject gameObject){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        gameObject.transform.Find("background").GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
        gameObject.transform.Find("center").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }
    public void pixelTintingGreen(GameObject gameObject){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(0,0.6f,0,1);
    }
    public void pixelTintingBlack(GameObject gameObject){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }
    public void pixelTintingInactive(GameObject gameObject){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(1,0,0,0);
    }
}
