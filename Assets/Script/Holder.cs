using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Holder : MonoBehaviour
{
    //홀더가 생성된 순번, SaveData 스크립트에 의해 초기화 됨
    public int num;
    //홀더의 타입
    public Panel_Holder.Type HolderType;
    //이 홀더가 포함하는 픽셀의 오브젝트 리스트
    public GameObject[] HavePixel;
    //이 홀더 배치 공정정보를 노출하는 기록라인
    public GameObject RecordLine;
    public int[] MinCrd;
    public int[] MaxCrd;

    //홀더의 타입에 따라 '픽셀 오브젝트 리스트'의 길이가 달라진다.q
    public void SetHavePixel(){
        if(HolderType == Panel_Holder.Type.Two || HolderType == Panel_Holder.Type.TwoVertical){
            this.HavePixel = new GameObject[2];
        }else if(HolderType == Panel_Holder.Type.Three || HolderType == Panel_Holder.Type.ThreeVertical){
            this.HavePixel = new GameObject[3];
        }else if(HolderType == Panel_Holder.Type.Five){
            this.HavePixel = new GameObject[25];
        }
    }
    
    //홀더에 포함된 좌표를 등록 및 좌표에 this 오브젝트 등록
    public void SetPixel(List<GameObject> gameObjects){
        //현재 홀더의 타입에 맞춰 배열 길이 셋팅
        SetHavePixel();
        //셋팅된 배열에 픽셀 저장
        for(int i = 0 ; i < gameObjects.Count ; i++){
            HavePixel[i] = gameObjects[i];
            //좌표의 오브젝트에 존재하는 값을 설정, 이 홀더에 포함되어 있음을 표시
            gameObjects[i].GetComponent<Pixel>().Holder = this.gameObject;
        }
    }

    
    //홀더 삭제될 때는 꼭 이 함수를 호출해서 삭제할 것!
    //안그러면 픽셀과 매칭되어있는 홀더 값을 제거할 수 없음!
    public void Destroy(){
        //'픽셀 오브젝트 리스트'에 존재하는 픽셀들의 정보(홀더 오브젝트)를 비움
        for(int i = 0 ; i < HavePixel.Length ; i++){
            HavePixel[i].GetComponent<Pixel>().Holder = null;
        }
        //기록 라인과 현재 오브젝트 파괴
        Destroy(RecordLine.gameObject);
        Destroy(this.gameObject);
    }

    public void ActiveObject(){
        gameObject.SetActive(true);
    }
    public void InactiveObject(){
        gameObject.SetActive(false);
    }
}
