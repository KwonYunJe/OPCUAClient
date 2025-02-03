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
    public bool IsPut;
    public bool IsErase;

    public RaycastHit[] hit;
    public RaycastHit hitOne;
    public int DetectedCount;
    public GameObject[] DetectedObject;
    

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
        // DrawRay();
        // DetectClick();
        // CanCreateObject();
        UpdateAble();
    }

    void UpdateAble(){
        if(!CursorOnPanel || CursorOnUI){
            return;
        }
        // SetMousePosition();
        // SetMouseProperty();
        GetKey();
        DrawRay();
        DetectClick();
        CanCreateObject();
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

    //마우스 위치에 Gizmo 생성
    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        
        Gizmos.DrawCube(center, boxSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, boxSize);
    }

    //Ray를 그리는 함수
    void DrawRay(){
        //박스로 픽셀을 판단 
        hit = Physics.BoxCastAll(center, boxSize / 2, Vector3.back, Quaternion.identity, 0);
        
        //Ray를 그려서 오브젝트를 판단, 마우스 위치에 아래의 오브젝트들이 존재하면 오브젝트를 지울 수 있음
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitOne)){
            if(hitOne.collider.tag == "Holder" || hitOne.collider.tag == "HolderPin" || hitOne.collider.tag == "Cell" || hitOne.collider.tag == "Nickel" || hitOne.collider.tag == "Welding"){
                IsErase = true;
            }else{  
                IsErase = false;
            }
        }

        if(hit.Length > 0){
            //Debug.Log(hit.Length);
            DetectedCount = hit.Length;
            DetectedObject = new GameObject[DetectedCount];
            for(int i = 0; i < DetectedCount; i++){
                DetectedObject[i] = hit[i].collider.gameObject;
            }
            // foreach(RaycastHit h in hit){
            //     Debug.Log(h.collider.name);
            // }
        }else{
            DetectedCount = 0;
            DetectedObject = null;
        }
    }

    //홀더별로 큐브가 생성될 center 및 크기 설정
    void SetMouseProperty(){
        if(GameManager.instance.property.taskType == Property.TaskType.Holder){
            switch(GameManager.instance.property.holderType){
            case Property.HolderType.Holder2x1:
            //2배열 짜리는 가운데가 달라서 아래와 같이 center를 설정해줘야 함
                center = new Vector3((float)Math.Round(mousePosition.x + 0.5f) - 0.5f, (float)Math.Round(mousePosition.y), 0);
                boxSize = new Vector3(1.9f , 0.9f , 0);
                break;
            case Property.HolderType.Holder1x2:
                center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y + 0.5f) - 0.5f, 0);
                boxSize = new Vector3(0.9f , 1.9f , 0);
                break;
            default:
                center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), 0);
                if(GameManager.instance.property.holderType == Property.HolderType.Holder3x1){
                    boxSize = new Vector3(2.9f , 0.9f , 0);
                }else if(GameManager.instance.property.holderType == Property.HolderType.Holder1x3){
                    boxSize = new Vector3(0.9f , 2.9f , 0);
                }else if(GameManager.instance.property.holderType == Property.HolderType.Holder5x5){
                    boxSize = new Vector3(4.9f , 4.9f , 0);
                }
                break;
            }
        }
    }

    //대충 픽셀 오브젝트 이외의 오브젝트가 탐지되면 작업 오브젝트를 생성할 수 없게 함
    void CanCreateObject(){
        if(DetectedCount == 0){
            IsPut = false;
        }else{
            foreach(GameObject obj in DetectedObject){
                if(obj.tag == "Holder" || obj.tag == "HolderPin" || obj.tag == "Cell" || obj.tag == "Nickel" || obj.tag == "Welding"){
                    IsPut = false;
                    return;
                }
            }
            IsPut = true;
        }
    }

    //클릭 감지
    void DetectClick(){
        if(Input.GetMouseButtonDown(0) && IsPut){
            if(CursorOnPanel){
                if(GameManager.instance.property.taskType == Property.TaskType.Holder){
                    GameManager.instance.task.CreateTask(new float[]{center.x, center.y});
                }
            }
        }else if(Input.GetMouseButtonDown(1)){
            GameManager.instance.task.EraseTask(hitOne.collider.gameObject);
        }
    }

    void GetKey(){
        if(Input.GetKeyDown("1")){
            GameManager.instance.property.SetProperty(0);
        }else if(Input.GetKeyDown("2")){
            GameManager.instance.property.SetProperty(1);
        }else if(Input.GetKeyDown("3")){
            GameManager.instance.property.SetProperty(2);
        }else if(Input.GetKeyDown("4")){
            GameManager.instance.property.SetProperty(3);
        }else if(Input.GetKeyDown("5")){
            GameManager.instance.property.SetProperty(4);
        }else if(Input.GetKeyDown(KeyCode.Tab)){
            GameManager.instance.property.ChangeTaskType();
        }
    }
}
