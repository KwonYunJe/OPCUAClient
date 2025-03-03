using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager: MonoBehaviour
{
    public GameObject Panel;
    public GameObject PinPanel;
    public GameObject[] Holder;
    public GameObject[] HolderPin;
    public GameObject[] Cell;
    public GameObject[] Nickel;
    public GameObject[] Welding;   

    public GameObject Holders;
    public GameObject HolderPins;
    public GameObject Cells;
    public GameObject Nickels;
    public GameObject Weldings;

    private void Start() {
        GameManager.instance.property.ChangeTask += ActivateObject;
    }


    //유형에 따른 오브젝트 생성 함수
    public GameObject InstantiateObject(Property.TaskType _taskType, int _taskID, float _x, float _y){

        GameObject _taskObject = null;
        
        switch(_taskType){
            case Property.TaskType.Holder:
                _taskObject = Instantiate(Holder[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                _taskObject.transform.SetParent(Holders.transform);
                break;
            case Property.TaskType.Pin:
                _taskObject = Instantiate(HolderPin[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                _taskObject.transform.SetParent(HolderPins.transform);
                break;
            case Property.TaskType.Cell:
                _taskObject = Instantiate(Cell[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                _taskObject.transform.SetParent(Cells.transform);
                break;
            case Property.TaskType.Welding:
                _taskObject = Instantiate(Welding[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                break;
        }
        return _taskObject;
    }

    public GameObject InstantiateObject(GameObject[] _objects){
        GameObject obj = new GameObject("Nickel");
        obj.tag = "Nickel";

        foreach(GameObject _object in _objects){
            GameObject CreateNickel = Instantiate(Nickel[0], new Vector3(_object.transform.position.x, _object.transform.position.y, 0), Quaternion.identity);
            CreateNickel.transform.SetParent(obj.transform);
        }

        return obj;
    }

    public void DestroyObject(GameObject _gameObject){
        Collider[] _colliders =_gameObject.GetComponent<Object_Script>().StartDestory();
        StartCoroutine(DelayDestroy(_colliders));
    }

    IEnumerator DelayDestroy(Collider[] _colliders){
        yield return new WaitForSeconds(0.001f);
        InitPixels(_colliders);
    }

    public void InitPixels(Collider[] _colliders){
        foreach(Collider _col in _colliders){
            _col.GetComponent<Pixels>().ScanObj();
        }
    }

    public void ActivateObject(Property.TaskType _taskType){
        // Cells.SetActive(false);
        // Nickels.SetActive(false);
        // Weldings.SetActive(false);

        // switch(_taskType){
        //     case Property.TaskType.Holder:
        //         Panel.SetActive(true);
        //         break;
        //     case Property.TaskType.Pin:
        //         PinPanel.SetActive(true);
        //         break;
        //     case Property.TaskType.Cell:
        //         Cells.SetActive(true);
        //         break;
        // }
    }
}
