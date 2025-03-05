using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickel_Script : Object_Script
{
    private void Start() {

    }

    public override Collider[] StartDestory(){
        int index = 0;
        foreach(Transform child in transform)
        {
            GameManager.instance.objectsManager.DestroyObject(child.gameObject);
            if(index == transform.childCount - 1){
                Destroy(gameObject);
            }else{
                index++;
            }
        }
        return new Collider[0];
    }

}

