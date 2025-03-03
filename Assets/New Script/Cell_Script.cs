using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Script : Object_Script
{
    private void Start() {
        SetLayerMask();
        SetScale();
        ScanObj();
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(ThisScale.x, ThisScale.y, 0.2f));
    }

    private void OnDestroy() {
        Debug.Log("셀 오브젝트가 파괴되었습니다.");   
    }    

    override public void SetScale(){
        ThisScale = transform.GetChild(0).localScale;
    }
}
