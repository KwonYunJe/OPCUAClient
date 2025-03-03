using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Pixels : MonoBehaviour
{
    [SerializeField]
    public Collider[] colliders;
    // public GameObject Holder;
    // public GameObject Cell;
    // public GameObject Nickel;
    // public GameObject Welding;
    LayerMask layerMask;
    public GameObject tint;
    public GameObject selectedtint;

    private void Start() {
        //홀더와 셀 레이어만 감지
        layerMask = (1 << LayerMask.NameToLayer("holder")) + (1 << LayerMask.NameToLayer("cell")) + (1 << LayerMask.NameToLayer("nickel")) + (1 << LayerMask.NameToLayer("welding"));
        //클릭 이벤트 등록
    
    }

    private void Update() {
    }

    public void InitColliders(){
        Debug.Log("픽셀 초기화 함수 실행");
        Array.Clear(colliders, 0, colliders.Length);
    }

    // public void StartScanObj(){
    //     Invoke("ScanObj", 0.001f);
    // }
    public void ScanObj(){
        Debug.Log("픽셀 감지 함수 실행");
        colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f), transform.rotation, layerMask);
        
        SetTint();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }

    //오브젝트가 감지되고 있으면 색을 어둡게
    private void SetTint(){
        if(colliders.Length > 0){
            tint.SetActive(true);
        }else{
            tint.SetActive(false);
        }
    }

    public void SetTintSelected(bool _bool){
        selectedtint.SetActive(_bool);
    }

    public bool IsHolder(){
        if(Array.Exists(colliders, x => x.tag == "Holder")){
            return true;
        }else{
            return false;
        }
    }
}
