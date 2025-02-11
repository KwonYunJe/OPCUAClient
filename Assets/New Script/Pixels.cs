using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Pixels : MonoBehaviour
{
    public Collider[] colliders;
    LayerMask layerMask;
    public GameObject tint;

    private void Start() {
        layerMask = (1 << LayerMask.NameToLayer("holder")) + (1 << LayerMask.NameToLayer("cell"));
    }

    private void Update() {
        DetectObj();
        SetTint();
    }

    private void DetectObj(){
        colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f), transform.rotation, layerMask);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }

    private void SetTint(){
        if(colliders.Length > 0){
            tint.SetActive(true);
        }else{
            tint.SetActive(false);
        }
    }
}
