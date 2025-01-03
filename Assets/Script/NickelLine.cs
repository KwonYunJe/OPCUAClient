using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//니켈의 한 프로세스가 저장되는 객체
public class NickelLine
{
    //니켈라인을 이루는 니켈 한 칸들이 저장되는 리스트 
    public List<Nickel> nickels;
    //니켈라인의 타입을 저장
    public Panel_Nickel.Type NickelType;
    //해당 프로세스와 연계되는 기록라인
    public GameObject RecordLine;

    //니켈 프로세스를 삭제할 때는 아래 메서드를 사용해야함
    public void DeleteProcess(){
        for(int i = nickels.Count - 1 ; i >= 0 ; i--){
            Nickel delNickel = nickels[i];
            nickels.Remove(delNickel);
            RecordLine.GetComponent<RecordLine>().DeleteLine();
            delNickel.Destroy();
        }
        nickels.Clear();
        
    }

    //니켈 프로세스 프리뷰 재생을 위한 오브젝트 활성화 메서드 
    public void ActiveObject(){
        foreach(Nickel _nickel in nickels){
            _nickel.gameObject.SetActive(true);
        }
    }
    //니켈 프로세스 프리뷰 재생을 위한 오브젝트 비활성화 메서드 
    public void InactiveObject(){
        foreach(Nickel _nickel in nickels){
            _nickel.gameObject.SetActive(false);
        }
    }
}
