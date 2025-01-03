using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class MouseEvent : MonoBehaviour
{
    //존재하는 모든 스크립트와 오브젝트가 담긴 allScript
    public GameObject allScript;
    //Panel 오브젝트
    private Panel_Holder panel_Holder;
    private Panel_Pin panel_Pin;
    private Panel_Cell panel_Cell;
    private Panel_Nickel panel_Nickel;
    private Panel_Welding panel_Welding;
    //mouse 위치를 담는 변수
    public Vector3 mousePosition;
    //Ray가 부딛힐때 정보를 담는 변수
    private RaycastHit hit;

    //클릭을 할 수 있는지
    public bool CanClickPanel;
    //커서가 UI위에 존재하는지
    public bool CursorOnUI;
    public bool PickingWelding;

    public bool Movealbe;

    // Start is called before the first frame update
    void Start()
    {
        panel_Holder = allScript.GetComponent<AllScript>().GetPanel_Holder();
        panel_Pin = allScript.GetComponent<AllScript>().GetPanel_Pin();
        panel_Cell = allScript.GetComponent<AllScript>().GetPanel_Cell();
        panel_Nickel = allScript.GetComponent<AllScript>().GetPanel_Nickel();
        panel_Welding = allScript.GetComponent<AllScript>().GetPanel_Welding();
    }

    // Update is called once per frame
    void Update()
    {
        MouseIn();
        ClickEvent();
        UIDetect();
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, - Camera.main.transform.position.z));
        SelectWeldingPixel();
    }

    //UI를 감지하는 함수
    void UIDetect(){
        CursorOnUI = EventSystem.current.IsPointerOverGameObject();
    }


    //마우스 커서의 위치 감지
    void MouseIn(){
        //커서가 UI위에 존재하면 이하의 함수 실행하지 않음
        if(CursorOnUI || Movealbe == true){
            return;
        }
        if(mousePosition.x <= 15.5f && mousePosition.x >= -0.5f && mousePosition.y <= 11.5f && mousePosition.y >=-0.5f){
            //클릭 가능
            CanClickPanel = true; 
            MouseHovering();
            
        }else{
            //다음 함수를 실행( 패널의 색, 현재 좌표값 초기화 )
            panel_Holder.initValue();
            panel_Pin.initValue();
            panel_Cell.initValue();
            panel_Nickel.initValue();
            //패널 클릭 불가능
            CanClickPanel = false;
        }  
    }


    //클릭감지
    void ClickEvent(){
        //커서가 UI위에 존재하면 이하의 함수 실행하지 않음
        if(CursorOnUI){
            return;
        }
        //마우스 왼쪽클릭을 감지
        if(Input.GetMouseButtonUp(0)){
            Debug.Log("마우스 왼쪽클릭 감지됨");
            //패널을 클릭할 수 있는 상태라면
            switch(allScript.GetComponent<ChangeMode>().ScreenMode){
                case ChangeMode.ModeOption.Holder:
                    if(CanClickPanel){
                        panel_Holder.SelectPixel();
                    }
                break;
                case ChangeMode.ModeOption.HolderPin:
                    if(CanClickPanel){
                        panel_Pin.SelectPixel();
                    }
                break;
                case ChangeMode.ModeOption.Cell:
                    if(CanClickPanel){
                        panel_Cell.SelectPixel();
                    }
                break;
                case ChangeMode.ModeOption.Nickel:
                    if(CanClickPanel){
                        panel_Nickel.SelectPixel();
                    }
                break;
            }
            
        }
        //마우스 오른쪽 클릭을 감지
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("마우스 오른쪽클릭 감지됨");
            switch(allScript.GetComponent<ChangeMode>().ScreenMode){
                case ChangeMode.ModeOption.Holder:
                    if(CanClickPanel){
                        panel_Holder.Reverse();
                    }
                break;
                case ChangeMode.ModeOption.HolderPin:
                    if(CanClickPanel){
                        panel_Pin.Reverse();
                    }
                break;
                case ChangeMode.ModeOption.Cell:
                    if(CanClickPanel){
                        panel_Cell.Reverse();
                    }
                break;
                case ChangeMode.ModeOption.Nickel:
                    if(CanClickPanel){
                        panel_Nickel.Reverse();
                    }
                break;
                case ChangeMode.ModeOption.Welding:
                    if(CanClickPanel){
                        panel_Welding.Reverse();
                    }
                break;
            }
        }
    }

    //마우스를 올려둘때만 발생
    public void MouseHovering(){
        //마우스 커서 위치에 Ray 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //위 Ray에 감지되는 것이 있다면
        if(Physics.Raycast(ray, out hit)){
            switch(allScript.GetComponent<ChangeMode>().ScreenMode){
                case ChangeMode.ModeOption.Holder:
                //충돌한 개체의 태그가 pixel 이면
                if(hit.collider.tag == "pixel"){
                    //충돌한 콜라이더가 있는 오브젝트를 다음 메서드로 전달
                    panel_Holder.pickedPixel(hit.collider.gameObject);
                }
                break;
                case ChangeMode.ModeOption.HolderPin:
                if(hit.collider.tag == "Pin"){
                    panel_Pin.pickedPixel(hit.collider.gameObject);
                }else{
                    panel_Pin.unPickedPixel();
                }
                break;
                case ChangeMode.ModeOption.Cell:
                if(hit.collider.tag == "CellPixel"){
                    panel_Cell.pickedPixel(hit.collider.gameObject);
                }else{
                    panel_Cell.initValue();
                }
                break;
                case ChangeMode.ModeOption.Nickel:
                if(hit.collider.tag == "NickelPixel"){
                    panel_Nickel.pickPixel(hit.collider.gameObject);
                }else{
                    panel_Nickel.initValue();
                }
                break;
                case ChangeMode.ModeOption.Welding:
                if(hit.collider.tag == "WeldingPixel"){
                    PickingWelding = true;
                    try{
                        panel_Welding.ViewInfo(hit.collider.gameObject);
                    }catch(Exception err){
                        Debug.Log(err);
                    }
                
                }else{
                    
                }
                break;
            }
                
        }else{
            panel_Pin.unPickedPixel();
            panel_Cell.initValue();
        }
    }

    public void SelectWeldingPixel(){
        if(Input.GetMouseButton(0)){

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit)){
                if(hit.collider.gameObject.tag == "WeldingPixel"){
                    switch(allScript.GetComponent<ChangeMode>().ScreenMode){
                        case ChangeMode.ModeOption.Welding:
                            panel_Welding.AddPickedPixels(hit.collider.gameObject);
                        break;
                    }
                }
                
            }
        }else if(Input.GetMouseButtonUp(0) && PickingWelding == true){
            panel_Welding.SetPixel();
            PickingWelding = false;
        }
    }
}
