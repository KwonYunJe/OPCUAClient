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
        TaskUnit addTaskUnit = new TaskUnit(Property.TaskType.Holder, taskID, _crdn[0], _crdn[1]);

        switch(taskType){
            case Property.TaskType.Holder:
                GameManager.instance.taskDATA.HolderTaskList.Add(addTaskUnit);
                break;
            case Property.TaskType.HolderPin:
                GameManager.instance.taskDATA.PinTaskList.Add(addTaskUnit);
                break;
            case Property.TaskType.Cell:
                GameManager.instance.taskDATA.CellTaskList.Add(addTaskUnit);
                break;
            case Property.TaskType.Nickel:
                GameManager.instance.taskDATA.NickelTaskList.Add(addTaskUnit);
                break;
            case Property.TaskType.Welding:
                GameManager.instance.taskDATA.WeldingTaskList.Add(addTaskUnit);
                break;
        }
    }

    public void EraseTask(GameObject _gameObject){
        //1. 넘어오는 게임 오브젝트는 모드마다 다를것이므로 현재 모드에 맞춰 리스트를 선택해 줄 필요 없음.
        //1-1. 아 빡대가린갑다. 넘어오는 오브젝트의 타입으로 리스트를 고르던 지금 모드로 리스트를 고르던 해야함.
        //2. 넘겨받은 오브젝트로 리스트에서 해당 오브젝트를 찾는다.
        //3. 찾은 오브젝트가 담긴 Task를 삭제한다.
        //4. 해당 오브젝트를 삭제한다.
        //내일 하자.
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


