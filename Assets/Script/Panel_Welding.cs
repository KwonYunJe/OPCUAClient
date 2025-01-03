using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Panel_Welding : MonoBehaviour
{
    public AllScript allScript;
    public GameObject[,] pixel = new GameObject[12,16] ;

//선택된 픽셀
    public List<GameObject> pickedPixels ;


    private void Start() {
        startPutArray();
    }

    private void startPutArray(){
        for(int i = 0 ; i < 12 ; i++){
            for(int j= 0 ; j < 16 ; j++){
                pixel[i,j] = gameObject.transform.GetChild(i).transform.GetChild(j).gameObject;
            }
        } 
    }


    public void ActiveObject(List<GameObject> _activeList){ 
        for(int i = 0 ; i < _activeList.Count ; i++){
            int x = (int)_activeList[i].transform.position.x;
            int y = (int)_activeList[i].transform.position.y;

            pixel[y,x].SetActive(true);
        }
    }
    public void AddPickedPixels(GameObject _pixel){
        if(pickedPixels == null || !pickedPixels.Contains(_pixel)){
            Debug.Log("추가됨 : " + _pixel.name);
            pickedPixels.Add(_pixel);
            pixelTintingRed(pickedPixels);
        }
    }

    public void SetPixel(){
        if(pickedPixels.Count == 0){
            Debug.Log("리스트 비었음");
            return;
        }else{
            Debug.Log("리스트 작업 시작");
            pixelTintingGreen(pickedPixels);
            foreach(GameObject _pixel in pickedPixels){
                Debug.Log(_pixel.name);
                Debug.Log(_pixel.GetComponent<WeldingPixel>().Point);
                Debug.Log(allScript.GetUIController().WeldingValue[0]);
                _pixel.GetComponent<WeldingPixel>().Point = allScript.GetUIController().WeldingValue[0];
                _pixel.GetComponent<WeldingPixel>().Time = allScript.GetUIController().WeldingValue[1];
                _pixel.GetComponent<WeldingPixel>().Current = allScript.GetUIController().WeldingValue[2];
                _pixel.GetComponent<WeldingPixel>().Press = allScript.GetUIController().WeldingValue[3];
                allScript.GetComponent<SaveData>().AddWeldingProcess(_pixel.GetComponent<WeldingPixel>());
            }
            pickedPixels.Clear();
        }
    }

    public void Reverse(){
        if(allScript.GetComponent<SaveData>().GetWeldingProcessCount() > 0){
            allScript.GetComponent<SaveData>().DeleteLastIndex_Welding();
        }else{
            Debug.Log("더 이상 되돌릴 수 없습니다.");
        }
    }

    public void ViewInfo(GameObject _gameObject){
        allScript.GetComponent<AllScript>().GetUIController().SetWeldingProcessInfo(_gameObject.GetComponent<WeldingPixel>());
    }

    public void pixelTintingGreen(List<GameObject> objects){
        pixelTintingDefault(objects);
        for(int i = 0 ; i < objects.Count; i++){
            SpriteRenderer spriteRenderer = objects[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0, 1, 0, 0.3f);
        }
    }

    public void pixelTintingRed(List<GameObject> objects){
        pixelTintingDefault(objects);
        for(int i = 0 ; i < objects.Count; i++){
            SpriteRenderer spriteRenderer = objects[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 0, 0, 0.3f);
        }
    }

    public void pixelTintingDefault(List<GameObject> objects){
        for(int i = 0 ; i < objects.Count; i++){
            SpriteRenderer spriteRenderer = objects[i].transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        }
    }

}
