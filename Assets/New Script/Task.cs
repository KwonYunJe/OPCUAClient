using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using OpenCover.Framework.Model;
using Unity.Properties;
using UnityEngine;
using System.Linq;

public class Task
{
    //픽셀정보를 통해서 홀더가 존재하는지 체크 
    public void CheckPixel(GameObject _gameObject){
        if(GameManager.instance.property.taskType == Property.TaskType.Pin){
            if(!_gameObject.GetComponent<PinPixels>().Ready){
                Debug.Log("The point is not ready");
            }else{
                CreateTask(new float[]{_gameObject.transform.position.x, _gameObject.transform.position.y});
            }
        }else{
            if(!_gameObject.GetComponent<Pixels>().IsHolder()){
            Debug.Log("Holder is not placed");
            }else{
                CreateTask(new float[]{_gameObject.transform.position.x, _gameObject.transform.position.y});
            }
        }
    }

    public void CheckPixel(GameObject[] _gameObjects){
        if(GameManager.instance.property.taskType == Property.TaskType.Nickel){
            CreateTask(_gameObjects);
        }
    }

    public void CreateTask(float[] crdn){
        //알아서 현재 작업중인 모드의 어떤 오브젝트를 놓는지 반환됨
        int taskID = GameManager.instance.property.GetNowTaskTypeID();
        //현재 작업중인 모드
        Property.TaskType _taskType = GameManager.instance.property.taskType;
        //Task를 만들고 TaskList에 추가
        TaskUnit taskUnit = new TaskUnit(_taskType, taskID, crdn[0], crdn[1]);
        GameManager.instance.taskDATA.taskListMap[_taskType.ToString()].Add(taskUnit);
    }
    public void CreateTask(GameObject[] gameObjects){
        //감지된 오브젝트를 x, y축 기준 오름차순 정렬
        Array.Sort(gameObjects, (a, b) => {
            int result = a.transform.position.x.CompareTo(b.transform.position.x);
            return result == 0 ? a.transform.position.y.CompareTo(b.transform.position.y) : result;
        });
        //알아서 현재 작업중인 모드의 어떤 오브젝트를 놓는지 반환됨
        int taskID = GameManager.instance.property.GetNowTaskTypeID();
        //현재 작업중인 모드
        Property.TaskType _taskType = GameManager.instance.property.taskType;

        TaskUnitNickel taskUnit = new TaskUnitNickel(_taskType, taskID, gameObjects);
        GameManager.instance.taskDATA.taskListMap[_taskType.ToString()].Add(taskUnit);
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

public class TaskUnitNickel : TaskUnit
{
    public TaskUnitNickel(Property.TaskType _taskType, int _taskID, GameObject[] _objects) : base(_taskType, _taskID, 0, 0)
    {
        TaskObject = GameManager.Instance.objectsManager.InstantiateObject(_objects);
        Debug.Log(TaskObject.name);
        ActivateArm();
    }

    void ActivateArm(){
        List<Transform> allTransforms = new List<Transform>();

        foreach(Transform child in TaskObject.transform)
        {
            allTransforms.Add(child);
        }

        Transform[] allChildren = allTransforms.ToArray();

        Debug.Log(allChildren.Length);
        foreach(Transform child in allChildren){
            Debug.Log(child.name);
            if(Array.Exists(allChildren, childOne => childOne.transform.position == new Vector3(child.position.x + 1, child.position.y, 0))){
                child.GetComponent<NickelPoint_Script>().ArmRight();
            }

            if(Array.Exists(allChildren, childOne => childOne.transform.position == new Vector3(child.position.x - 1, child.position.y, 0))){
                child.GetComponent<NickelPoint_Script>().ArmLeft();
            }

            if(Array.Exists(allChildren, childOne => childOne.transform.position == new Vector3(child.position.x, child.position.y + 1, 0))){
                child.GetComponent<NickelPoint_Script>().ArmTop();
            }

            if(Array.Exists(allChildren, childOne => childOne.transform.position == new Vector3(child.position.x, child.position.y - 1, 0))){
                child.GetComponent<NickelPoint_Script>().ArmBottom();
            }
        }
    }
}

// class CreateTask{
//     float[] crdn = new float[2];
//     public CreateTask(float[] crdn){
//         this.crdn = crdn;
//         Task();
//     }
//     //알아서 현재 작업중인 모드의 어떤 오브젝트를 놓는지 반환됨
//     int taskID = GameManager.instance.property.GetNowTaskTypeID();
//     //현재 작업중인 모드
//     Property.TaskType _taskType = GameManager.instance.property.taskType;
//     //Task를 만들고 TaskList에 추가
//     void Task(){
//         TaskUnit taskUnit = new TaskUnit(_taskType, taskID, crdn[0], crdn[1]);
//         GameManager.instance.taskDATA.taskListMap[_taskType.ToString()].Add(taskUnit);
//     }
// }

class EraseTask{
    public EraseTask(GameObject _gameObject){
        if(_gameObject.tag == "Nickel"){
            
        }
        // 태그에 맞는 리스트가 있는지 확인
        else if (GameManager.instance.taskDATA.taskListMap.TryGetValue(_gameObject.tag, out List<TaskUnit> taskList)) {
            // // 해당 오브젝트의 콜라이더를 저장(픽셀 콜라이더)
            // 리스트에서 해당 오브젝트를 찾고 삭제
            for (int i = 0; i < taskList.Count; i++) {
                if (taskList[i].TaskObject == _gameObject) {
                    //_col = _gameObject.GetComponent<Object_Script>().colliders;
                    // 오브젝트 삭제
                    GameManager.instance.objectsManager.DestroyObject(_gameObject);
                    break;
                }
            }
            // 해당 오브젝트를 포함하는 픽셀의 정보를 초기화
            // foreach(Collider _cols in _col){
            //     _cols.gameObject.GetComponent<Pixels>().StartCoroutine("InitColliders");
            // }
        } else {
            Debug.LogWarning("Unknown tag: " + _gameObject.tag);
        }
    }
}




