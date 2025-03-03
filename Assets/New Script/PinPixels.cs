using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinPixels : MonoBehaviour
{
    public bool Ready;
    public Collider[][] colliders1 = new Collider[4][];

    LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("holder");
        GameManager.instance.property.ChangeTask += DelayScan;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DelayScan(Property.TaskType _taskType){
        if(_taskType == Property.TaskType.Pin){
            Debug.Log("핀 픽셀 감지 함수 실행");
            Invoke("ScanObj", 0.001f);
        }
    }

    void ScanObj(){
        //if(GameManager.instance.property.taskType != Property.TaskType.Pin){return;}
        //Debug.Log("핀 감지 함수 실행");
        Ready = false;
        InitAAA();
        colliders1[0] = Physics.OverlapBox(new Vector3(transform.position.x -0.1f,transform.position.y -0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f), transform.rotation, layerMask);
        colliders1[1] = Physics.OverlapBox(new Vector3(transform.position.x -0.1f,transform.position.y + 0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f), transform.rotation, layerMask);
        colliders1[2] = Physics.OverlapBox(new Vector3(transform.position.x + 0.1f,transform.position.y - 0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f), transform.rotation, layerMask);
        colliders1[3] = Physics.OverlapBox(new Vector3(transform.position.x + 0.1f,transform.position.y + 0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f), transform.rotation, layerMask);
        //Debug.Log(colliders1[0].Length + ", " + colliders1[1].Length + ", " + colliders1[2].Length + ", " + colliders1[3].Length);
        SetReady();
    }

    void SetReady(){
        
        Ready = false;
        for(int i = 0 ; i < 4 ; i++){
            if(colliders1[i].Length != 1 || !colliders1[i][0].gameObject.CompareTag("Holder")){
                return;
            }else if(i == 3){
                Ready = true;
            }
        }
    }

    public void InitAAA(){
        Array.Clear(colliders1, 0, colliders1.Length);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x -0.1f,transform.position.y -0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(transform.position.x -0.1f,transform.position.y + 0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + 0.1f,transform.position.y - 0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f));
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + 0.1f,transform.position.y + 0.1f, 0), new Vector3(0.05f, 0.05f, 0.1f));
    }
}
