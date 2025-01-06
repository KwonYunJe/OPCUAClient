using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Vector3 mousePosition;
    public Vector3Int mousePositionInt;

    public Vector3 center;
    public bool CursorOnUI;
    public bool CursorOnPanel;
    public bool Movealbe;
    public int[] GizmoVal = new int[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectCursorOnUI();
        DetectCursorOnPanel();
        SetMousePosition();
        SetMouseProperty();
    }

    //마우스 커서의 위치를 감지하는 함수
    void SetMousePosition(){
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - Camera.main.transform.position.z));
        mousePositionInt = new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0);
    }

    //UI를 감지하는 함수
    void DetectCursorOnUI(){
        CursorOnUI = EventSystem.current.IsPointerOverGameObject();
    }

    //패널을 감지하는 함수
    void DetectCursorOnPanel(){
        if(mousePosition.x <= 15.5f && mousePosition.x >= -0.5f && mousePosition.y <= 11.5f && mousePosition.y >=-0.5f){
            //패널 위에 존재
            CursorOnPanel = true;
        }else{
            //패널 밖에 존재
            CursorOnPanel = false;
        }
    }

    //마우스 위치에 큐브 생성
    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        
        Gizmos.DrawCube(center, new Vector3(GizmoVal[0], GizmoVal[1], GizmoVal[2]));
    }

    void SetMouseProperty(){
        if(GameManager.instance.property.holderType == Property.HolderType.Holder2x1){
            center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), 0);
            GizmoVal[0] = 2;
            GizmoVal[1] = 1;
            GizmoVal[2] = 0;
            }
    }
}
