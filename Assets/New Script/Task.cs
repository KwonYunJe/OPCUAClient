using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Task
{
    //픽셀정보를 통해서 홀더가 존재하는지 체크 
    public void CheckPixel(GameObject _gameObject){
        if(GameManager.instance.property.taskType == Property.TaskType.Pin){
            if(!_gameObject.GetComponent<PinPixels>().Ready){
                Debug.Log("The point is not ready");
            }else{
                new CreateTask(new float[]{_gameObject.transform.position.x, _gameObject.transform.position.y});
            }
        }else{
            if(_gameObject.GetComponent<Pixels>().Holder == null){
            Debug.Log("Holder is not placed");
            }else{
                new CreateTask(new float[]{_gameObject.transform.position.x, _gameObject.transform.position.y});
            }
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

class CreateTask{
    float[] crdn = new float[2];
    public CreateTask(float[] crdn){
        this.crdn = crdn;
        Task();
    }
    //알아서 현재 작업중인 모드의 어떤 오브젝트를 놓는지 반환됨
    int taskID = GameManager.instance.property.GetNowTaskTypeID();
    //현재 작업중인 모드
    Property.TaskType _taskType = GameManager.instance.property.taskType;
    //Task를 만들고 TaskList에 추가
    void Task(){
        TaskUnit taskUnit = new TaskUnit(_taskType, taskID, crdn[0], crdn[1]);
        GameManager.instance.taskDATA.taskListMap[_taskType.ToString()].Add(taskUnit);
    }
}

class EraseTask{
    public EraseTask(GameObject _gameObject){
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




