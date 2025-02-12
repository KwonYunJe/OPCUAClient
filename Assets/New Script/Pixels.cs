using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Pixels : MonoBehaviour
{
    public Collider[] colliders;
    public GameObject Holder;
    public GameObject Cell;
    public GameObject Nickel;
    public GameObject Welding;
    LayerMask layerMask;
    public GameObject tint;

    private void Start() {
        //홀더와 셀 레이어만 감지
        layerMask = (1 << LayerMask.NameToLayer("holder")) + (1 << LayerMask.NameToLayer("cell")) + (1 << LayerMask.NameToLayer("nickel")) + (1 << LayerMask.NameToLayer("welding"));
        //클릭 이벤트 등록
        InputManager inputManager = GameManager.Instance.inputManager;
        inputManager.ClickAction += DetectObj;
    }

    private void Update() {
        
    }

    //현재 셀 위에 존재하는 오브젝트 감지
    private void DetectObj(){
        if(GameManager.instance.property.taskType != Property.TaskType.Cell){return;}
        Debug.Log("픽셀 감지 함수 실행");
        colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f), transform.rotation, layerMask);
        SetObjects();
    }

    //클릭이 발생하면 감지되고 있는 오브젝트를 변수에 초기화
    private void SetObjects(){
        Holder = null;
        Cell = null;
        Nickel = null;
        Welding = null;

        foreach(Collider col in colliders){
            if(col.gameObject.CompareTag("Holder")){
                Holder = col.gameObject;
            }else if(col.gameObject.CompareTag("Cell")){
                Cell = col.gameObject;
            }else if(col.gameObject.CompareTag("Nickel")){
                Nickel = col.gameObject;
            }else if(col.gameObject.CompareTag("Welding")){
                Welding = col.gameObject;
            }
        }
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
}
