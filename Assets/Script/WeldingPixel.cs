using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldingPixel : MonoBehaviour
{
    public bool Setting;
    public float Point;
    public float Time;
    public float Current;
    public float Press;

    public GameObject RecordLine;

    private void Update() {
        Set();    
    }

    public void Set(){
        if(Point != 0 && Time != 0 && Current != 0 && Press != 0){
            Setting = true;
        }else{
            Setting = false;
        }
    }

    public void Init(){
        Point = 0;
        Time = 0;
        Current = 0;
        Press = 0;
        SpriteRenderer spriteRenderer = transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
    }

    public void ReActiveObject(){
        SpriteRenderer spriteRenderer = transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0, 1, 0, 0.3f);
    }
    public void InactiveObject() {
        SpriteRenderer spriteRenderer = transform.Find("tint").gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
    }
}
