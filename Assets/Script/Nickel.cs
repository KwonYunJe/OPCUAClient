using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickel : MonoBehaviour
{
    public int num;
    public Panel_Nickel.Type NickelType;
    public GameObject HavePixel;

    // 0 = Top / 1 = Bottom / 2  = Left / 3 = Right
    public GameObject[] NickelArm;

    public void SetPixel(GameObject _pixel){
        HavePixel = _pixel;
    }
    public void Destroy(){
        HavePixel.GetComponent<Nickel_Pixel>().Nickel = null;
        Destroy(gameObject);
    }

    public void ActiveUp(){
        NickelArm[0].SetActive(true);
    }
    public void ActiveDown(){
        NickelArm[1].SetActive(true);
    }
    public void ActiveLeft(){
        NickelArm[2].SetActive(true);
    }
    public void ActiveRight(){
        NickelArm[3].SetActive(true);
    }

}
