using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//AllScript오브젝트에 GameObject를 등록하기 위한 스크립트
//해당 스크립트는 GameObject를 등록하고 다른 스크립트에서 또다른 스크립트나 오브젝트로 접근할 때 사용함
public class AllScript : MonoBehaviour
{
    public GameObject UI;
    public GameObject panel_Holder;
    public GameObject panel_Pin;
    public GameObject panel_Cell;
    public GameObject panel_Nickel;
    public GameObject panel_Welding;
    public GameObject MouseListener;
    public GameObject PreviewPlayPanel;
    public GameObject Camera;

    public UIController GetUIController(){
        return UI.GetComponent<UIController>();
    }
    public Panel_Holder GetPanel_Holder(){
        return panel_Holder.GetComponent<Panel_Holder>();
    }
    public Panel_Pin GetPanel_Pin(){
        return panel_Pin.GetComponent<Panel_Pin>();
    }
    public Panel_Cell GetPanel_Cell(){
        return panel_Cell.GetComponent<Panel_Cell>();
    }
    public Panel_Nickel GetPanel_Nickel(){
        return panel_Nickel.GetComponent<Panel_Nickel>();
    }
    public Panel_Welding GetPanel_Welding(){
        return panel_Welding.GetComponent<Panel_Welding>();
    }
    public MouseEvent GetMouseListener(){
        return MouseListener.GetComponent<MouseEvent>();
    }
    
}

