using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMode : MonoBehaviour
{
    public enum ModeOption{Holder, HolderPin, Cell, Nickel, Welding}
    public ModeOption ScreenMode;
    public static UIController uIController;
    public TMP_Dropdown dropdown;





    public void Start(){
        uIController = gameObject.GetComponent<AllScript>().GetUIController();


    }

    public void ChangeToMode(int _index){

        bool changeUI = false;
        bool HolderPicKButtonActive = false;
        bool CellPickButtonActive = false;
        bool PinAutoSetButtonActive = false;
        bool NickelPickButtonActive = false;
        bool WeldingStatusButtonActive = false;

        List<GameObject> HolderInfo = new List<GameObject>();

        switch(_index){
            case 0:
                //홀더 종류 선택 버튼 켜기
                HolderPicKButtonActive = true;
                //스크린 모드 변경
                ScreenMode = ModeOption.Holder;
                //핀 공정 정보 초기화
                gameObject.GetComponent<AllScript>().panel_Pin.GetComponent<Panel_Pin>().allInActive();
                //셀 공정 정보 초기화
                gameObject.GetComponent<AllScript>().panel_Cell.GetComponent<Panel_Cell>().allInActive();
                //니켈 배치 공정 정보를 초기화
                gameObject.GetComponent<AllScript>().panel_Nickel.GetComponent<Panel_Nickel>().allInActive();

                //홀더 배치 과정에서 비활성화 됐던 오브젝트들 다시 활성화
                gameObject.GetComponent<AllScript>().panel_Holder.GetComponent<Panel_Holder>().activeOther();
            break;
            case 1:
                changeUI = true;

                PinAutoSetButtonActive = true;
                //홀더가 놓여지지 않은 픽셀들을 가시적으로 비활성화 처리하고 오브젝트가 놓인 픽셀들을 반환 받음
                HolderInfo = gameObject.GetComponent<AllScript>().panel_Holder.GetComponent<Panel_Holder>().inActiveOther();   
                //위에서 받은 '오브젝트가 존재하는 픽셀'들을 기반으로 핀이 놓일 픽셀들을 활성화 시킴
                gameObject.GetComponent<AllScript>().panel_Pin.GetComponent<Panel_Pin>().ActiveObject(HolderInfo);
                //스크린 모드 변경
                ScreenMode = ModeOption.HolderPin;
                //셀의 공정 정보를 초기화
                gameObject.GetComponent<AllScript>().panel_Cell.GetComponent<Panel_Cell>().allInActive();
                //니켈 배치 공정 정보를 초기화
                gameObject.GetComponent<AllScript>().panel_Nickel.GetComponent<Panel_Nickel>().allInActive();
            break;
            case 2:
                changeUI = true;
                //셀 종류 선택 버튼 켜기
                CellPickButtonActive = true;
                //스크린 모드 변경
                ScreenMode = ModeOption.Cell;

                //홀더가 놓여지지 않은 픽셀들을 가시적으로 비활성화 처리하고 오브젝트가 놓인 픽셀들을 반환 받음
                HolderInfo = gameObject.GetComponent<AllScript>().panel_Holder.GetComponent<Panel_Holder>().inActiveOther();  
                //위에서 받은 '오브젝트가 존재하는 픽셀'들을 기반으로 셀이 놓일 픽셀들을 활성화 시킴
                gameObject.GetComponent<AllScript>().panel_Cell.GetComponent<Panel_Cell>().ActiveObject(HolderInfo);
                //니켈 배치 공정 정보를 초기화
                gameObject.GetComponent<AllScript>().panel_Nickel.GetComponent<Panel_Nickel>().allInActive();
            break;
                
            case 3:
                changeUI = true;

                NickelPickButtonActive = true;

                ScreenMode = ModeOption.Nickel;

                HolderInfo = gameObject.GetComponent<AllScript>().panel_Holder.GetComponent<Panel_Holder>().inActiveOther();  
                gameObject.GetComponent<AllScript>().panel_Nickel.GetComponent<Panel_Nickel>().ActiveObject(HolderInfo);

            break;

            case 4:
                changeUI = true;

                WeldingStatusButtonActive = true; 

                ScreenMode = ModeOption.Welding;

                HolderInfo = gameObject.GetComponent<AllScript>().panel_Holder.GetComponent<Panel_Holder>().inActiveOther();  
                gameObject.GetComponent<AllScript>().panel_Welding.GetComponent<Panel_Welding>().ActiveObject(HolderInfo);

            break;

        }
        if(changeUI){
            //스크롤뷰 변경
            uIController.SetScrollView(_index);
            //머티리얼 정보 변경
            uIController.SetMaterialInfoActive(_index);
        }
        
        uIController.HolderPickButton.SetActive(HolderPicKButtonActive);
        uIController.PinAutoSetButton.SetActive(PinAutoSetButtonActive);
        uIController.CellPickButton.SetActive(CellPickButtonActive);
        uIController.NickelPickButton.SetActive(NickelPickButtonActive);
        uIController.WeldingStatus.SetActive(WeldingStatusButtonActive);
    }


}
