using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickelPoint_Script : Object_Script
{
    public GameObject[] Arm;
    // Start is called before the first frame update
    void Start()
    {
        SetLayerMask();
        SetScale();
        ScanObj();
    }

    public override void SetScale(){
        ThisScale = transform.Find("Circle").localScale;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }

    private void OnDestroy() {
        Debug.Log("니켈 오브젝트가 파괴되었습니다.");   
    }    

    public void ArmRight(){
        Arm[3].SetActive(true);
    }

    public void ArmLeft(){
        Arm[2].SetActive(true);
    }

    public void ArmTop(){
        Arm[0].SetActive(true);
    }

    public void ArmBottom(){
        Arm[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
