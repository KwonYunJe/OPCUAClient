using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Vector3 mousePosition;

    public Property.TaskType taskType;
    public Vector3 center;
    public Vector3 boxSize;
    public bool CursorOnUI;
    public bool CursorOnPanel;
    public bool IsPut;
    public bool IsErase;
    public int layerMask;
    public int MaxDetectedCount;

    [SerializeField]
    public RaycastHit[] hit;
    public RaycastHit hitOne;
    public RaycastHit hitTwo;
    public int DetectedCount;
    public GameObject DetectedObject;
    public GameObject[] DetectedObjects;
    public GameObject HitOneObject;
    public GameObject NowObject;

    public NickelSelect nickelSelect;

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
        UpdateAble();

        ChangeTaskTyope();
        SetMaxDetectedCount();
        DetectMove();

        taskType = GameManager.instance.property.taskType;
        
    }

    void UpdateAble(){
        if(!CursorOnPanel || CursorOnUI){
            return;
        }
        GetKey();
        DrawRay();
        DetectClick();
        IsInteractionObject();
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

    //작업 타입에 따라 레이어 마스크 변경
    void ChangeTaskTyope(){
        if(GameManager.instance.property.taskType == Property.TaskType.Holder){
            layerMask = (1 << LayerMask.NameToLayer("pixel")) + (1 << LayerMask.NameToLayer("holder"));
        }else if(GameManager.instance.property.taskType == Property.TaskType.Pin){
            layerMask = 1 << LayerMask.NameToLayer("pinPixel");
        }else if(GameManager.instance.property.taskType == Property.TaskType.Cell){
            layerMask = (1 << LayerMask.NameToLayer("pixel")) + (1 << LayerMask.NameToLayer("cell"));
        }else if(GameManager.instance.property.taskType == Property.TaskType.Nickel){
            //layerMask = 1 << LayerMask.NameToLayer("pixel");
            layerMask = (1 << LayerMask.NameToLayer("pixel")) + (1 << LayerMask.NameToLayer("nickel"));
        }else if(GameManager.instance.property.taskType == Property.TaskType.Welding){
            layerMask = (1 << LayerMask.NameToLayer("pixel")) + (1 << LayerMask.NameToLayer("welding"));
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
        hit = Physics.BoxCastAll(center, boxSize / 2, Vector3.back, Quaternion.identity, default ,layerMask);

        if(hit.Length > 0){
            //Debug.Log(hit.Length);
            DetectedCount = hit.Length;
            DetectedObjects = new GameObject[DetectedCount];
            for(int i = 0; i < DetectedCount; i++){
                DetectedObjects[i] = hit[i].collider.gameObject;
            }
            // foreach(RaycastHit h in hit){
            //     Debug.Log(h.collider.name);
            // }
        }else{
            DetectedCount = 0;
            DetectedObjects = null;
        }

        //Ray를 그려서 오브젝트를 판단, 셀 한 개에 오브젝트를 놓을 때 사용
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hitOne, 100, layerMask)){
            HitOneObject = hitOne.collider.gameObject;
        }else{
            HitOneObject = null;
        }

        //Ray를 그려서 오브젝트를 판단, 오브젝트를 삭제할 때 사용
        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray1, out hitTwo)){
            //HitTwoObject = hitTwo.collider.gameObject;
        }else{
            //HitTwoObject = null;
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
                case Property.HolderType.Holder3x1:
                    center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), 0);
                    boxSize = new Vector3(2.9f , 0.9f , 0);
                    break;
                case Property.HolderType.Holder1x3:
                    center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), 0);
                    boxSize = new Vector3(0.9f , 2.9f , 0);
                    break;
                case Property.HolderType.Holder5x5:
                    center = new Vector3((float)Math.Round(mousePosition.x), (float)Math.Round(mousePosition.y), 0);
                    boxSize = new Vector3(4.9f , 4.9f , 0);
                    break;
            }
        }else if(GameManager.instance.property.taskType == Property.TaskType.Nickel){
            switch(GameManager.instance.property.nickelType){
                case Property.NickelType.Nickel_1:
                    center = new Vector3(7.5f, (float)Math.Round(mousePosition.y), 0);
                    boxSize = new Vector3(15.9f , 0.9f , 0);
                    break;
                case Property.NickelType.Nickel_2:
                    center = new Vector3(7.5f, (float)Math.Round(mousePosition.y + 0.5f) - 0.5f, 0);
                    boxSize = new Vector3(15.9f , 1.9f , 0);
                    break;
                case Property.NickelType.Nickel_3:
                    center = new Vector3(7.5f, (float)Math.Round(mousePosition.y), 0);
                    boxSize = new Vector3(15.9f , 2.9f , 0);
                    break;
                case Property.NickelType.Nickel_4:
                    center = new Vector3(7.5f, (float)Math.Round(mousePosition.y + 0.5f) - 0.5f, 0);
                    boxSize = new Vector3(15.9f , 3.9f , 0);
                    break;
            }
            
        }else{
            center = Vector3.zero;
            boxSize = Vector3.zero;
        }
    }

    //홀더를 놓을 때 최대로 탐지할 수 있는 픽셀의 개수 설정
    void SetMaxDetectedCount(){
        if(GameManager.instance.property.taskType == Property.TaskType.Holder){
            if(GameManager.instance.property.holderType == Property.HolderType.Holder2x1 || GameManager.instance.property.holderType == Property.HolderType.Holder1x2){
                MaxDetectedCount = 2;
            }else if(GameManager.instance.property.holderType == Property.HolderType.Holder3x1 || GameManager.instance.property.holderType == Property.HolderType.Holder1x3){
                MaxDetectedCount = 3;

            }else if(GameManager.instance.property.holderType == Property.HolderType.Holder5x5){
                MaxDetectedCount = 25;
            }else{
                MaxDetectedCount = 0;
            }
        }else if(GameManager.instance.property.taskType == Property.TaskType.Nickel){
            if(GameManager.instance.property.nickelType == Property.NickelType.Nickel_1){
                MaxDetectedCount = 16;
            }else if(GameManager.instance.property.nickelType == Property.NickelType.Nickel_2){
                MaxDetectedCount = 32;
            }else if(GameManager.instance.property.nickelType == Property.NickelType.Nickel_3){
                MaxDetectedCount = 48;
            }else if(GameManager.instance.property.nickelType == Property.NickelType.Nickel_4){
                MaxDetectedCount = 64;
            }else{
                MaxDetectedCount = 0;
            }
        }
    }

    //대충 픽셀 오브젝트 이외의 오브젝트가 탐지되면 작업 오브젝트를 생성할 수 없게 함
    void IsInteractionObject(){
        //오브젝트가 ray에 걸리는지(단일) 여부 판단해서 오브젝트 '생성' 가능 여부 결정
        if(hitOne.collider != null){
            switch(GameManager.instance.property.taskType){
                case Property.TaskType.Holder:
                    if(hitOne.collider.tag == "pixel"){
                        IsPut = true;
                    }
                    break;
                case Property.TaskType.Pin:
                    if(hitOne.collider.tag == "pinPixel"){
                        IsPut = true;
                    }
                    break;
                case Property.TaskType.Cell:
                    if(hitOne.collider.tag == "pixel"){
                        IsPut = true;
                    }
                    break;
                case Property.TaskType.Nickel:
                    if(hitOne.collider.tag == "pixel"){
                        IsPut = true;
                    }
                    break;
                case Property.TaskType.Welding:
                    IsPut = false;
                    break;
                default:
                    IsPut = false;
                    break;
            }
        }else{
            IsPut = false;
        }

        //오브젝트가 ray에 걸리는지(범위) 여부 판단해서 오브젝트 '생성' 가능 여부 결정
        if(GameManager.instance.property.taskType == Property.TaskType.Holder){
            if(DetectedCount == MaxDetectedCount && hitOne.collider != null){
                foreach(GameObject obj in DetectedObjects){
                    if(obj.tag != "pixel"){
                        IsPut = false;
                        return;
                    }
                }
                IsPut = true;
            }else {
                IsPut = false;
            }
        }else if(GameManager.instance.property.taskType == Property.TaskType.Nickel){
            if(hitOne.collider != null){
                foreach(GameObject obj in DetectedObjects){
                    if(obj.tag != "pixel"){
                        IsPut = false;
                        return;
                    }
                }
                IsPut = true;
            }else {
                IsPut = false;
            }

        }

        //오브젝트가 ray에 걸리는지 여부 판단해서 오브젝트 '삭제' 가능 여부 결정
        if(hitTwo.collider == null){
            IsErase = false;
        }else{
            if(hitTwo.collider.tag == "Holder" || hitTwo.collider.tag == "Pin" || hitTwo.collider.tag == "Cell" || hitTwo.collider.tag == "NickelPoint" || hitTwo.collider.tag == "Welding"){
                IsErase = true;
            }else{
                IsErase = false;
            }
        }
    }

    //클릭 감지
    void DetectClick(){
        if(Input.GetMouseButtonUp(0) && IsPut){
            //클릭 이벤트 발생
            switch(GameManager.instance.property.taskType){
                case Property.TaskType.Holder:
                    if(hitOne.collider.tag != "pixel"){ return; }
                    GameManager.instance.task.CreateTask(new float[]{center.x, center.y});
                    break;
                case Property.TaskType.Pin:
                    if(hitOne.collider.tag != "pinPixel"){ return; }
                    //Debug.Log("Pin pos : " + hitOne.collider.transform);
                    GameManager.instance.task.CheckPixel(hitOne.collider.gameObject);
                    break;
                case Property.TaskType.Cell:
                    if(hitOne.collider.tag != "pixel"){ return; }
                    //Debug.Log("cell pos : " + hitOne.collider.transform);
                    //위 두 개는 홀더의 배치와 독립적인 배치 작업이므로 바로 작업 오브젝트를 생성하지만 
                    //셀은 홀더의 유무에 따라 배치되어야 하므로 홀더 존재를 파악하는 함수를 거쳐야 함.
                    GameManager.instance.task.CheckPixel(hitOne.collider.gameObject);
                    break;
                case Property.TaskType.Nickel:
                    if(hitOne.collider.tag != "pixel"){ return; }
                    //Debug.Log(nickelSelect.OnMouseLine.Length);
                    GameManager.instance.task.CheckPixel(nickelSelect.OnMouseLine);
                    break;
                case Property.TaskType.Welding:
                    
                    break;
            }
        }else if(Input.GetMouseButtonUp(1) && IsErase){
            new EraseTask(hitTwo.collider.gameObject);
        }
    }

    void DetectMove(){
        if(GameManager.instance.property.taskType == Property.TaskType.Nickel){
            if(NowObject == null || HitOneObject != NowObject){
                HitOneObject = NowObject;
                // if(nickelSelect != null){
                //     nickelSelect.ClearLine();
                // }
                nickelSelect = new NickelSelect(hit);
            }
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
        }else if(Input.GetKeyDown("6")){
            GameManager.instance.property.SetProperty(5);

        }else if(Input.GetKeyDown(KeyCode.Tab)){
            GameManager.instance.property.ChangeTaskType();
        }
    }

    void SelectWeldingPoint(){
        if(Input.GetMouseButtonUp(0)){
            if(hitOne.collider.tag == "pixel"){

            }
        }
    }
}

public class NickelSelect{
    public GameObject[] OnMouseLine;
    public NickelSelect(RaycastHit[] _OnMousePixel){
        //Debug.Log("NickelSelect" + "Count : " + _OnMousePixel.Length);
        List<GameObject> pixelList = new List<GameObject>();
        foreach(RaycastHit _hit in _OnMousePixel){
            if(_hit.collider.tag != "pixel"){
                return;
            }else if(pixelList != null){
                if(_hit.collider.gameObject.GetComponent<Pixels>().IsHolder()){
                    pixelList.Add(_hit.collider.gameObject);
                }
            }
            
        }
        OnMouseLine = pixelList.ToArray();
        //TintLine();
    }

}

public class WeldingSelect{
    public List<GameObject> SelectedOjbects = new List<GameObject>();

    public void AddWeldingPoint(GameObject _pixel){
        if(SelectedOjbects == null || !SelectedOjbects.Contains(_pixel)){
            SelectedOjbects.Add(_pixel);
        }
    }

    public void SetWeldingPoint(){
        if(SelectedOjbects.Count == 0){
            Debug.Log("리스트 비었음");
            return;
        }else{
            foreach(GameObject _pixel in SelectedOjbects){
                
            }
            SelectedOjbects.Clear();
        }
    }
}
