using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object_Script : MonoBehaviour
{    public Collider[] colliders;

    public LayerMask layerMask;

    public Vector3 ThisScale;

    public GameObject RecordObj;

    public void SetLayerMask(){
        switch(gameObject.tag){
            case "Holder":
                layerMask = 1 << LayerMask.NameToLayer("pixel");
                break;
            case "Pin":
                layerMask = 1 << LayerMask.NameToLayer("pinPixel");
                break;
            case "Cell":
                layerMask = 1 << LayerMask.NameToLayer("pixel");
                break;
            case "Nickel":
                layerMask = 1 << LayerMask.NameToLayer("pixel");
                break;
            case "Welding":
                layerMask = 1 << LayerMask.NameToLayer("pixel");
                break;

        }
    }

    virtual public void SetScale(){
        ThisScale = transform.Find("background").localScale;
    }

    virtual public void ScanObj(){
        Debug.Log("오브젝트가 감지 함수 실행");
        colliders = Physics.OverlapBox(transform.position, new Vector3(ThisScale.x/2, ThisScale.y/2, 0.1f), transform.rotation, LayerMask.GetMask("pixel"));
        Debug.Log("감지 크기 : " + ThisScale.x + ", " + ThisScale.y);
        Debug.Log("감지된 오브젝트 수 : " + colliders.Length);
        StartScanObj();
    }

    public void StartScanObj(){
        foreach(Collider _col in colliders){
            _col.GetComponent<Pixels>().ScanObj();
        }
    }

    public void DeleteRecord()
    {
        if (RecordObj != null)
        {
            Destroy(RecordObj); // Viewport의 Record 한 줄 삭제
        }
    }

    //오브젝트 삭제 시작
    virtual public Collider[] StartDestory(){
        int index = 0;
        DeleteRecord();
        foreach(Collider _col in colliders){
            _col.GetComponent<Pixels>().InitColliders();
            if(index == colliders.Length - 1){
                Destroy(gameObject);
            }else{
                index++;
            }
        }
        return colliders;
    }

}
