using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Panel_Cell : MonoBehaviour
{

    public GameObject allScript;
    public UIController uIController;
    public SaveData saveData;
    public GameObject pickedCell;
    public GameObject[,] pixel = new GameObject[12,16] ;

    public enum Type { plus50E, minus50E, plusM50, minusM50, plus40T, minus40T } ;
    public Type CellType;
    public GameObject[] Cell;
    public bool CanSelectPanel;




    // Start is called before the first frame update
    void Start()
    {
        uIController = allScript.GetComponent<AllScript>().UI.GetComponent<UIController>();
        saveData = allScript.GetComponent<SaveData>();
        putArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void putArray(){
        Debug.Log(gameObject.transform.childCount);
        Debug.Log(gameObject.transform.GetChild(0).childCount);

        for(int i = 0 ; i< 12 ; i++){
            for(int j = 0 ; j < 16 ; j++){
                pixel[i,j] = gameObject.transform.GetChild(i).transform.GetChild(j).gameObject;
            }
        }
    }

//배치된 홀더에 따라 셀을 놓을 수 있는 부분을 활성화
    public void ActiveObject(List<GameObject> _activeList){ 
        for(int i = 0 ; i < _activeList.Count ; i++){
            int x = (int)_activeList[i].transform.position.x;
            int y = (int)_activeList[i].transform.position.y;

            pixel[y,x].SetActive(true);
        }
    }

//다른 모드로 변경될 때 모든 셀을 비활성화
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

//마우스커서가 셀을 둘 수 있는 자리에 올라가면 호출
//픽셀의 색상을 변경한다.
    public void pickedPixel(GameObject _gameObject){
        //선택한 픽셀에 이미 셀이 배치되어 있다면 아래 함수를 실행하지 않는다.
        if(_gameObject.GetComponent<Cell_Pixel>().Cell != null){
            return;
        }
        //마우스에 감지되어 넘어온 매개변수가 전역변수 오브젝트와 일치하지 않으면
        if(_gameObject != pickedCell){
            //pickedCell을 비우는 메서드를 호출한다.
            initValue();
        }
        //전역변수 오브젝트를 넘겨받은 오브젝트로 변경
        pickedCell = _gameObject;
        //해당 오브젝트의 색을 초록색으로 변경한다.
        pixelTintingGreen(pickedCell);
        //픽셀을 클릭하여 셀을 놓을 수 있는 플래그를 true로 변경
        CanSelectPanel = true;
    }
//마우스커서가 다른 픽셀로 이동하는 등 유효하지 않은 위치로 이동할 때 호출
//픽셀의 색상을 원래대로 되돌린다.
    public void initValue(){
        foreach(GameObject _pixel in pixel){
            //픽셀이 Null이 아니고 아무런 셀도 배치되지 않았다면
            if(_pixel != null && _pixel.GetComponent<Cell_Pixel>().Cell == null){
                //해당 픽셀을 원래 색으로 되돌린다.
                pixelTintingDefault(_pixel);
                //픽셀을 클릭하여 셀을 놓을수 없도록 플래그를 false로 변경한다.
                CanSelectPanel = false;
            }
        }
    }
//마우스 커서가 유효한 픽셀을 클릭했을 때 호출
//해당 위치에 셀 오브젝트를 배치한다.
//셀 픽셀의 색을 원래대로 바꾼다
//(어차피 셀 오브젝트에 가려지기 때문에 별 상관없을 것 같지만 
//이전 단계로 돌아가면 셀 오브젝트가 지워지기 때문에 원상복구가 필요함)
    public void SelectPixel(){
        if(CanSelectPanel){
            PutCell();
            pixelTintingDefault(pickedCell);
        }else{
            Debug.Log("선택할 수 없습니다.");
        }
    }

    public void PutCell(){
        if(pickedCell.GetComponent<Cell_Pixel>().Cell != null){
            return;
        }
        Vector3 nowCrd = pickedCell.transform.position;
        
        GameObject newCell = null;

        switch(CellType){
            case Type.plus50E:
                newCell = Instantiate(Cell[0], nowCrd, Cell[0].transform.rotation);
            break;
            case Type.minus50E:
                newCell = Instantiate(Cell[1], nowCrd, Cell[1].transform.rotation);
            break;
            case Type.plusM50:
                newCell = Instantiate(Cell[2], nowCrd, Cell[2].transform.rotation);
            break;
            case Type.minusM50:
                newCell = Instantiate(Cell[3], nowCrd, Cell[3].transform.rotation);
            break;
            case Type.plus40T:
                newCell = Instantiate(Cell[4], nowCrd, Cell[4].transform.rotation);
            break;
            case Type.minus40T:
                newCell = Instantiate(Cell[5], nowCrd, Cell[5].transform.rotation);
            break;
        }

        GameObject RecordLine = uIController.AddRecordLineCell(
            saveData.GetCellProcessCount(),
            newCell.GetComponent<Cell>().CellType,
            newCell.transform.position
        );

        newCell.GetComponent<Cell>().RecordLine = RecordLine;
        newCell.GetComponent<Cell>().SetPixel(pickedCell);
        saveData.AddCellProcess(newCell.GetComponent<Cell>());
        
    }

    public void Reverse(){
        if(saveData.GetCellProcessCount() > 0){
            saveData.DeleteLastIndex_Cell();
        }else{
            Debug.Log("더 이상 되돌릴 수 없습니다.");
        }
    }

    public void pixelTintingGreen(GameObject _picked){
        _picked.transform.Find("Circle").GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void pixelTintingDefault(GameObject _picked){
        _picked.transform.Find("Circle").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
    }
    public void pixelTintingBlue(GameObject _picked){
        _picked.transform.Find("Circle").GetComponent<SpriteRenderer>().color = Color.blue;
    }   
}
