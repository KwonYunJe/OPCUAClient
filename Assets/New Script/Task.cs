using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    //작업 시작
    public void CreateTask(float[] _crdn){
        Property.TaskType taskType = GameManager.instance.property.taskType;
        //TaskID는 enum의 순서를 이용하여 정의
        int taskID = 0 ;
        switch(GameManager.instance.property.taskType){
            case Property.TaskType.Holder:
                taskID = (int)GameManager.instance.property.holderType;
                break;
            case Property.TaskType.HolderPin:
                taskID = (int)GameManager.instance.property.pinType;
                break;
            case Property.TaskType.Cell:
                taskID = (int)GameManager.instance.property.cellType;
                break;
            case Property.TaskType.Nickel:
                taskID = (int)GameManager.instance.property.nickelType;
                break;
            case Property.TaskType.Welding:
                break;
        }

        switch(taskType){
            case Property.TaskType.Holder:
                TaskUnit addTaskUnit = new TaskUnit(Property.TaskType.Holder, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.HolderTaskList.Add(addTaskUnit);
                Debug.Log("Holder count : " + GameManager.instance.taskDATA.HolderTaskList.Count);
                break;
            case Property.TaskType.HolderPin:
                addTaskUnit = new TaskUnit(Property.TaskType.HolderPin, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.PinTaskList.Add(addTaskUnit);
                Debug.Log("HolderPin count : " + GameManager.instance.taskDATA.HolderTaskList.Count);
                break;
            case Property.TaskType.Cell:
                addTaskUnit = new TaskUnit(Property.TaskType.Cell, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.CellTaskList.Add(addTaskUnit);
                Debug.Log("Cell count : " + GameManager.instance.taskDATA.HolderTaskList.Count);
                break;
            case Property.TaskType.Nickel:
                addTaskUnit = new TaskUnit(Property.TaskType.Nickel, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.NickelTaskList.Add(addTaskUnit);
                Debug.Log("Nickel count : " + GameManager.instance.taskDATA.HolderTaskList.Count);
                break;
            case Property.TaskType.Welding:
                addTaskUnit = new TaskUnit(Property.TaskType.Welding, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.WeldingTaskList.Add(addTaskUnit);
                Debug.Log("Welding count : " + GameManager.instance.taskDATA.HolderTaskList.Count);
                break;
        }
    }

    public void EraseTask(GameObject _gameObject) {
        // 태그와 리스트를 매핑하는 딕셔너리 생성
        Dictionary<string, List<TaskUnit>> taskListMap = new Dictionary<string, List<TaskUnit>> {
            { "Holder", GameManager.instance.taskDATA.HolderTaskList },
            { "Pin", GameManager.instance.taskDATA.PinTaskList },
            { "Cell", GameManager.instance.taskDATA.CellTaskList },
            { "Nickel", GameManager.instance.taskDATA.NickelTaskList },
            { "Welding", GameManager.instance.taskDATA.WeldingTaskList }
        };

        // 태그에 맞는 리스트가 있는지 확인
        if (taskListMap.TryGetValue(_gameObject.tag, out List<TaskUnit> taskList)) {
            // 리스트에서 해당 오브젝트를 찾고 삭제
            for (int i = 0; i < taskList.Count; i++) {
                if (taskList[i].TaskObject == _gameObject) {
                    taskList.RemoveAt(i);
                    // 오브젝트 삭제
                    GameManager.instance.objectsManager.DestroyObject(_gameObject);
                    break;
                }
            }
        } else {
            Debug.LogWarning("Unknown tag: " + _gameObject.tag);
        }
    }
}

public class TaskDATA
{
    public List<TaskUnit> HolderTaskList = new List<TaskUnit>();
    public List<TaskUnit> PinTaskList = new List<TaskUnit>();
    public List<TaskUnit> CellTaskList = new List<TaskUnit>();
    public List<TaskUnit> NickelTaskList = new List<TaskUnit>();
    public List<TaskUnit> WeldingTaskList = new List<TaskUnit>();
}

public class TaskUnit
{
    public int TaskID;
    public Property.TaskType TaskType;
    public float[] Crdn = new float[2];
    public GameObject TaskObject;

    public TaskUnit(Property.TaskType _taskType, int _taskID, float _x, float _y){
        TaskType = _taskType;
        TaskID = _taskID;
        Crdn[0] = _x;
        Crdn[1] = _y;
        TaskObject = GameManager.Instance.objectsManager.InstantiateObject(_taskType, _taskID, _x, _y);
    }
}


