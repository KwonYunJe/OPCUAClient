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


    //유형에 따른 오브젝트 생성 함수
    public GameObject InstantiateObject(Property.TaskType _taskType, int _taskID, float _x, float _y){

        GameObject _taskObject = null;
        
        switch(_taskType){
            case Property.TaskType.Holder:
                _taskObject = Instantiate(Holder[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                break;
            case Property.TaskType.HolderPin:
                _taskObject = Instantiate(HolderPin[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                break;
            case Property.TaskType.Cell:
                _taskObject = Instantiate(Cell[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                break;
            case Property.TaskType.Nickel:
                _taskObject = Instantiate(Nickel[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                break;
            case Property.TaskType.Welding:
                _taskObject = Instantiate(Welding[_taskID], new Vector3(_x, _y, 0), Quaternion.identity);
                break;
        }

        return _taskObject;
    }
}
