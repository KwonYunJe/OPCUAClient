using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public bool isPicked;
    public GameObject RecordLine;
    public void SetEnable(bool _enable){
        gameObject.SetActive(_enable);
    }

    public void Reset(){
        isPicked = false;
        InactiveObject();
        Destroy(RecordLine.gameObject);
    }

    public void ActiveObject(){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
        gameObject.transform.Find("background").GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
        gameObject.transform.Find("center").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }
    public void InactiveObject(){
        gameObject.transform.Find("tint").GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
        gameObject.transform.Find("center").GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
        gameObject.transform.Find("background").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
    }


}
