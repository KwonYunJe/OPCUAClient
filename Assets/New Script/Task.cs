using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{

    public void StartTask(int[] _crdn){
        Property.TaskType taskType = GameManager.instance.property.taskType;
        TaskUnit AddTaskUnit = new TaskUnit(Property.TaskType.Holder, 0, _crdn[0], _crdn[1]);
    }

    public void EndTask(){
        
    }

    public void InstantiateObject(){
        
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
    public int[] Crdn = new int[2];
    public GameObject TaskObject;

    public TaskUnit(Property.TaskType _taskType, int _taskID, int _x, int _y){
        TaskType = _taskType;
        TaskID = _taskID;
        Crdn[0] = _x;
        Crdn[1] = _y;
        TaskObject = GameManager.Instance.objectsManager.InstantiateObject(_taskType, _taskID, _x, _y);
    }
}


