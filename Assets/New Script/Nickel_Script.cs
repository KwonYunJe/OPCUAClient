using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nickel___ : Object_Script
{
    private void Start() {

    }

    public override Collider[] StartDestory(){
        foreach(Transform child in transform)
        {
            GameManager.instance.objectsManager.DestroyObject(child.gameObject);
        }
        return new Collider[0];
    }

}

