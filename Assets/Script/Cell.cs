using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int num;
    public Panel_Cell.Type CellType;
    public GameObject RecordLine;
    public GameObject HavePixel;
    
    // Start is called before the first frame update
    public void SetPixel(GameObject gameObject){
        HavePixel = gameObject;
        gameObject.GetComponent<Cell_Pixel>().Cell = this.gameObject;
    }

    public void Destroy(){
        Destroy(RecordLine.gameObject);
        Destroy(gameObject);
    }

        public void ActiveObject(){
        gameObject.SetActive(true);
    }
    public void InactiveObject(){
        gameObject.SetActive(false);
    }
}
