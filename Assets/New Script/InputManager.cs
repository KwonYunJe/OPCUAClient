using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Vector3 mousePosition;
    //public Vector3 mousePosition2;

    public Vector3 center;
    public Vector3 boxSize;
    public bool CursorOnUI;
    public bool CursorOnPanel;
    public bool Movealbe;
    public int[] GizmoVal = new int[3];

    public RaycastHit[] hit;
    

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
        GetKey();
        DrawRay();
    }

    //마우스 커서의 위치를 감지하는 함수
    void SetMousePosition(){
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - Camera.main.transform.position.z));
        //mousePosition2 = new Vector3((int)mousePosition.x + 0.5f, (int)mousePosition.y, 0);
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
        
        Gizmos.DrawCube(center, boxSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, boxSize);
    }

    void DrawRay(){
        hit = Physics.BoxCastAll(center, boxSize / 2, Vector3.down, Quaternion.identity, 100);
        if(hit.Length > 0){
            //Debug.Log(hit.Length);
            foreach(RaycastHit h in hit){
                Debug.Log(h.collider.name);
            }
        }else{
        }
    }

    //홀더별로 큐브가 생성될 center 및 크기 설정
    void SetMouseProperty(){
        if(GameManager.instance.property.taskType == Property.TaskType.Holder){
            switch(GameManager.instance.property.holderType){
            case Property.HolderType.Holder2x1:
            //2배열 짜리는 가운데가 달라서 아래와 같이 center를 설정해줘야 함
                center = new Vector3((float)Math.Round(mousePosition.x + 0.5f) - 0.5f, (float)Math.Round(mousePosition.y), 0);
                boxSize = new Vector3(2 , 1 , 0.1f);
                break;
            case Property.HolderType.Holder1x2:
                center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y + 0.5f) - 0.5f, 0);
                boxSize = new Vector3(1 , 2 , 0.1f);
                break;
            default:
                center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), 0);
                if(GameManager.instance.property.holderType == Property.HolderType.Holder3x1){
                    boxSize = new Vector3(3 , 1 , 0.1f);
                }else if(GameManager.instance.property.holderType == Property.HolderType.Holder1x3){
                    boxSize = new Vector3(1 , 3 , 0.1f);
                }else if(GameManager.instance.property.holderType == Property.HolderType.Holder5x5){
                    boxSize = new Vector3(5 , 5 , 0.1f);
                }
                break;
            }
        }
        
    }

    void GetKey(){
        if(Input.GetKeyDown("1")){
            GameManager.instance.property.holderType = Property.HolderType.Holder2x1;
        }else if(Input.GetKeyDown("2")){
            GameManager.instance.property.holderType = Property.HolderType.Holder1x2;
        }else if(Input.GetKeyDown("3")){
            GameManager.instance.property.holderType = Property.HolderType.Holder3x1;
        }else if(Input.GetKeyDown("4")){
            GameManager.instance.property.holderType = Property.HolderType.Holder1x3;
        }else if(Input.GetKeyDown("5")){
            GameManager.instance.property.holderType = Property.HolderType.Holder5x5;
        }
    }
}
