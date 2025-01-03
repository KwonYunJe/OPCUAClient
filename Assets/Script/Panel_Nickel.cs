using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Panel_Nickel : MonoBehaviour
{
    public GameObject allScript;
    public UIController uIController;
    public SaveData saveData;
    public GameObject pickedNickel;
    public GameObject[,] pixel = new GameObject[12,16];
    public int xMax;    //x좌표의 최대 값
    public int yMax;    //y좌표의 최대 값
    public GameObject[,] activatePixel;

    public enum Type {Line1, Line2, Line3, Line4};
    public Type NickelType;
    public GameObject Nickel;
    public bool CanSelectPanel;
    //마우스를 올려뒀을 때 감지할 범위에 있는 니켈픽셀을 담을 리스트
    List<GameObject> pickedPixels = new();
    //니켈의 왼쪽아래 시작점과 오른쪽 상단 끝점을 저장하는 배열
    int[,] startAndLast;



    private void Start() {
        uIController = allScript.GetComponent<AllScript>().UI.GetComponent<UIController>();
        saveData = allScript.GetComponent<SaveData>();
        putArray();
    }

    //니켈 픽셀들을 배열에 담음
    public void putArray(){
        Debug.Log(gameObject.transform.childCount);
        Debug.Log(gameObject.transform.GetChild(0).childCount);

        for(int i = 0 ; i< 12 ; i++){
            for(int j = 0 ; j < 16 ; j++){
                pixel[i,j] = gameObject.transform.GetChild(i).transform.GetChild(j).gameObject;
            }
        }
    }

    //홀더가 놓여진 자리에 따라 니켈 픽셀들을 활성화 함
    public void ActiveObject(List<GameObject> _activeList){ 
        for(int i = 0 ; i < _activeList.Count ; i++){
            int x = (int)_activeList[i].transform.position.x;
            int y = (int)_activeList[i].transform.position.y;
            if(x > xMax){
                xMax = x;
            }
            if(y > yMax){
                yMax = y;
            }
            pixel[y,x].SetActive(true);
        }
    }

    //모든 니켈 픽셀들을 비활성화 함
    public void allInActive(){
        for(int i = 0 ; i < 12 ; i++){
            for(int j = 0 ; j < 16 ; j++){
                if(pixel[i,j].activeSelf){
                    pixel[i,j].SetActive(false);
                }
            }
        }
        saveData.ClearCellProcess();
    }

    //마우스가 니켈픽셀 위에 존재할 때 호출 됨
    public void pickPixel(GameObject _gameObject){
        if(_gameObject.GetComponent<Nickel_Pixel>().Nickel != null){
            return;
        }
        if(_gameObject != pickedNickel){
            //pickedCell을 비우는 메서드를 호출한다.
            initValue();
        }
        pickedNickel = _gameObject;

        //마우스로 짚은 오브젝트의 좌표를 담음
        float y = _gameObject.transform.position.y;

        //이미 니켈이 놓여있는지 여부를 알려줌
        bool nickelAlready = false;
        //마우스가 짚은 좌표가 홀더가 놓여있지 않은지 알려줌
        bool overPixelActive = false;

        int yStart = 0;
        int yEnd = 0;

        //{(x시작점,y시작점),(x끝점, y끝점)}
        startAndLast = new int[2,2];

        switch(NickelType){
            case Type.Line1:
                yStart = (int)y;
                yEnd = (int)y + 1;
            break;
            case Type.Line2:
                yStart = (int)y;
                yEnd = (int)y + 2;
            break;
            case Type.Line3:
                yStart = (int)y - 1;
                yEnd = (int)y + 2;
            break;
            case Type.Line4:
                yStart = (int)y - 1;
                yEnd = (int)y + 3;
            break;
        }

        for(int i = yStart ; i < yEnd ; i++){ 
            //활성화 되어있는 x축 상의 마지막 오브젝트까지 탐색
            for(int j = 0 ; j <= xMax ; j++){
                //탐색 오류가 발생시 (주로 인덱스 범위 이탈) 니켈을 배치할 수 없음
                try{
                    //니켈을 놓으려 하는 모든 좌표중 y값이 홀더가 놓인 범위를 벗어나면
                    if(i < 0 || i > yMax){
                        //범위가 벗어남을 알리는 boolean 변수를 true로 변경 
                        overPixelActive = true;
                    //범위가 벗어난 오브젝트는 변경할 필요가 없으므로 리스트에 담지 않는다.
                    //아래는 범위 안에 존재하는 오브젝트를 리스트에 저장한다.
                    }else if(pixel[i,j].activeSelf){
                        pickedPixels.Add(pixel[i,j]);
                        //리스트에 저장하는 오브젝트에 이미 니켈이 배치되어 있다면
                        if(pixel[i,j].GetComponent<Nickel_Pixel>().Nickel != null){
                            //니켈이 이미 배치되어 있음을 알리는 boolean 변수를 true로 변경
                            nickelAlready = true;
                        }
                    }
                }catch(Exception ex){
                    Debug.Log(i + ", " + j + " 좌표에는 픽셀이 존재하지 않습니다. 오류메시지 : " + ex.Message);
                    overPixelActive = true;
                }
            }
        }

        startAndLast[0,0] = (int)pickedPixels[0].transform.position.x;
        startAndLast[0,1] = (int)pickedPixels[0].transform.position.y;
        startAndLast[1,0] = (int)pickedPixels[pickedPixels.Count - 1].transform.position.x;
        startAndLast[1,1] = (int)pickedPixels[pickedPixels.Count - 1].transform.position.y;

        //이미 니켈이 배치되어있음을 의미하는 boolean변수가 거짓일 때 모든 픽셀을 초록색으로 변경
        if(!nickelAlready && !overPixelActive){
            for(int i = 0 ; i < pickedPixels.Count ; i++){
                pixelTintingGreen(pickedPixels[i]);
            }
            CanSelectPanel = true;
        //한 개라도 니켈이 배치되어 있음을 의미하는 boolean변수가 참일 때 모든 픽셀을 붉은색으로 변경
        }else{
            for(int i = 0 ; i < pickedPixels.Count ; i++){
                pixelTintingRed(pickedPixels[i]);
            }
            CanSelectPanel = false;
        }
    }

    public void initValue(){
        foreach(GameObject _pixel in pixel){
            //해당 픽셀을 원래 색으로 되돌린다.
            pixelTintingDefault(_pixel);
            //픽셀을 클릭하여 셀을 놓을수 없도록 플래그를 false로 변경한다.
            CanSelectPanel = false;
            //선택되어 있던 픽셀들이 담긴 리스트를 초기화
            pickedPixels.Clear();
            startAndLast = null;
            // //픽셀이 Null이 아니고 아무런 셀도 배치되지 않았다면
            // if(_pixel != null && _pixel.GetComponent<Nickel_Pixel>().Nickel == null){
                
            // }
        }
    }

    //마우스가 클릭됐을 때 호출
    public void SelectPixel(){
        if(CanSelectPanel){
            PutNickel();
            pixelTintingDefault(pickedNickel);
        }else{
            Debug.Log("선택할 수 없습니다.");
        }
    }


    //니켈 놓는 함수
    public void PutNickel(){
        if(pickedNickel.GetComponent<Nickel_Pixel>().Nickel != null){
            return;
        }
        //NicelLine은 니켈한개가 줄을 이루는 리스트와 타입, 기록라인을 가지는 객체
        NickelLine nickelLine = new NickelLine();
        //생성된 니켈 한 좌표를 담을 라인 (즉, 이 리스트 한 개가 니켈 한줄을 의미함.), NickelLine 객체의 필드 값으로 지정될 것임.
        List<Nickel> nickels = new();

        //(전역 변수로 선언된) 마우스가 가리키고 있는 픽셀들을 순환
        foreach(GameObject _pickedPixel in pickedPixels){
            //탐색중인 픽셀의 x좌표
            int x = (int)_pickedPixel.transform.position.x;
            //탐색중인 픽셀의 y좌표
            int y = (int)_pickedPixel.transform.position.y;
            //위 위치를 기반으로 니켈라인의 한 칸을 생성
            GameObject oneNickel = Instantiate(Nickel, new Vector3(x, y, 0), Quaternion.identity);
            //탐색중인 픽셀에 윗 줄에서 생성한 니켈 한 칸을 할당
            oneNickel.GetComponent<Nickel>().SetPixel(_pickedPixel);
            //반대로 생성한 니켈 한 칸에 탐색중인 픽셀을 할당
            _pickedPixel.GetComponent<Nickel_Pixel>().Nickel = oneNickel;
            //니켈 한 칸을 리스트에 저장
            
            nickels.Add(oneNickel.GetComponent<Nickel>());
        }

        //생성된 니켈 라인을 한 칸씩 순차적으로 탐색
        foreach(Nickel _nickel in nickels){
            int x = (int)_nickel.gameObject.transform.position.x;
            int y = (int)_nickel.gameObject.transform.position.y;

            //탐색중인 니켈의 상,하,좌,우 에 '같은 니켈라인'에 속하는 칸이 존재하면 각 니켈의 arm을 활성화
            if(y - 1 >= 0 && pickedPixels.Contains(pixel[y - 1 ,x])){
                _nickel.ActiveDown();
            }
            if(y + 1 < 12 && pickedPixels.Contains(pixel[y + 1 ,x])){
                _nickel.ActiveUp();
            }
            if(x + 1 < 16 && pickedPixels.Contains(pixel[y ,x + 1])){
                _nickel.ActiveRight();
            }
            if(x - 1 >= 0 && pickedPixels.Contains(pixel[y ,x - 1])){
                _nickel.ActiveLeft();
            }
        }

        //니켈리스트를 니켈라인 객체의 필드값으로 지정
        nickelLine.nickels = nickels;
        //니켈라인의 타입을 지정
        nickelLine.NickelType = NickelType;
        //기록라인 생성 및 니켈라인에 할당
        nickelLine.RecordLine = uIController.AddRecordLineNickel(saveData.GetNickelProcessCount() + 1, NickelType, startAndLast);
        //니켈라인을 프로세스에 저장
        saveData.AddNickelProcess(nickelLine);
        
    }

    public void Reverse(){
        if(saveData.GetNickelProcessCount() > 0){
            saveData.DeleteLastIndex_Nickel();
        }else{
            Debug.Log("더 이상 되돌릴 수 없습니다.");
        }
    }

    public void pixelTintingDefault(GameObject _picked){
        _picked.transform.Find("background").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        _picked.transform.Find("background (1)").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    public void pixelTintingGreen(GameObject _picked){
        _picked.transform.Find("background").GetComponent<SpriteRenderer>().color = Color.green;
        _picked.transform.Find("background (1)").GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void pixelTintingRed(GameObject _picked){
        _picked.transform.Find("background").GetComponent<SpriteRenderer>().color = Color.red;
        _picked.transform.Find("background (1)").GetComponent<SpriteRenderer>().color = Color.red;
    }

}
