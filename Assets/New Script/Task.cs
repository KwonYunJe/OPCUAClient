using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
                TaskUnit addTaskUnit = new TaskUnit(Property.TaskType.Holder, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.taskListMap["Holder"].Add(addTaskUnit);
                Debug.Log("Holder count : " + GameManager.instance.taskDATA.taskListMap["Holder"].Count);
                break;
            case Property.TaskType.HolderPin:
                addTaskUnit = new TaskUnit(Property.TaskType.HolderPin, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.taskListMap["Pin"].Add(addTaskUnit);
                Debug.Log("Pin count : " + GameManager.instance.taskDATA.taskListMap["Pin"].Count);
                break;
            case Property.TaskType.Cell:
                addTaskUnit = new TaskUnit(Property.TaskType.Cell, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.taskListMap["Cell"].Add(addTaskUnit);
                Debug.Log("Cell count : " + GameManager.instance.taskDATA.taskListMap["Cell"].Count);
                break;
            case Property.TaskType.Nickel:
                addTaskUnit = new TaskUnit(Property.TaskType.Nickel, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.taskListMap["Nickel"].Add(addTaskUnit);
                Debug.Log("Nickel count : " + GameManager.instance.taskDATA.taskListMap["Nickel"].Count);
                break;
            case Property.TaskType.Welding:
                addTaskUnit = new TaskUnit(Property.TaskType.Welding, taskID, _crdn[0], _crdn[1]);
                GameManager.instance.taskDATA.taskListMap["Welding"].Add(addTaskUnit);
                Debug.Log("Welding count : " + GameManager.instance.taskDATA.taskListMap["Welding"].Count);
                break;
        }
    }

    public void EraseTask(GameObject _gameObject) {
        // 태그에 맞는 리스트가 있는지 확인
        if (GameManager.instance.taskDATA.taskListMap.TryGetValue(_gameObject.tag, out List<TaskUnit> taskList)) {
            // 리스트에서 해당 오브젝트를 찾고 삭제
            for (int i = 0; i < taskList.Count; i++) {
                if (taskList[i].TaskObject == _gameObject) {
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
    // 태그와 리스트를 매핑하는 딕셔너리 생성
    public Dictionary<string, List<TaskUnit>> taskListMap = new Dictionary<string, List<TaskUnit>> {
        { "Holder", new List<TaskUnit>() },
        { "Pin", new List<TaskUnit>() },
        { "Cell", new List<TaskUnit>() },
        { "Nickel", new List<TaskUnit>() },
        { "Welding", new List<TaskUnit>() }
    };
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


