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

    //한개의 오브젝트가 들어왔을 때
    public void CreateTask(float[] crdn){
        //알아서 현재 작업중인 모드의 어떤 오브젝트를 놓는지 반환됨
        int taskID = GameManager.instance.property.GetNowTaskTypeID();

        //현재 작업중인 모드
        Property.TaskType _taskType = GameManager.instance.property.taskType;

        //TaskUnit 생성
        TaskUnit taskUnit = new TaskUnit(_taskType, taskID, crdn[0], crdn[1]);
        
        //TaskList에 추가
        GameManager.instance.taskDATA.taskListMap[_taskType.ToString()].Add(taskUnit);
    }

    //여러개의 오브젝트가 들어왔을 때(니켈)
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
    public GameObject RecordLine { get; private set; }

    public TaskUnit() {} // 기본 생성자 추가

    public TaskUnit(Property.TaskType _taskType, int _taskID, float _x, float _y){
        TaskType = _taskType;
        TaskID = _taskID;
        Crdn[0] = _x;
        Crdn[1] = _y;
        GameObject _record = GameManager.Instance.uiManager.CreateRecord(GameManager.Instance.taskDATA.taskListMap[_taskType.ToString()].Count.ToString(), _taskID.ToString(), "" + _x + ", " + _y);
        TaskObject = GameManager.Instance.objectsManager.InstantiateObject(_taskType, _taskID, _x, _y, _record);
    }


    // TaskUnit 생성 시 Record를 연결
    public void LinkRecord(GameObject _gameObject){
        RecordLine = _gameObject;
    }
    // 여기서 TaskUnit 자체 삭제 처리 (Task 목록에서도 제거)
}

public class TaskUnitNickel : TaskUnit
{
    public TaskUnitNickel(Property.TaskType _taskType, int _taskID, GameObject[] _objects) : base(_taskType, _taskID, 0, 0)
    {
        GameObject _record = RecordValue(_taskType, _taskID, _objects);
        TaskObject = GameManager.Instance.objectsManager.InstantiateObject(_objects, _record);
        Debug.Log(TaskObject.name);
        ActivateArm();
    }

    GameObject RecordValue(Property.TaskType _taskType, int _taskID, GameObject[] _objects){

        string num = GameManager.instance.taskDATA.taskListMap[_taskType.ToString()].Count.ToString();

        string first = _taskID.ToString();

        string second = "";

        if(_objects.Length % 2 == 0){
            second = "" + (_objects[_objects.Length / 2].transform.position.x + _objects[_objects.Length / 2 + 1].transform.position.x) / 2 + 
            ", " + (_objects[_objects.Length / 2].transform.position.y + _objects[_objects.Length / 2 + 1].transform.position.y) / 2;
        }else{
            second = "" +  _objects[_objects.Length / 2].transform.position.x + ", " + _objects[_objects.Length / 2].transform.position.y;
        }

        return GameManager.Instance.uiManager.CreateRecord(num, first, second);
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

public class WeldingTask : TaskUnit
{
    public float Point;
    public float Time;
    public float Current;
    public float Press;

    public WeldingTask(Property.TaskType _taskType, float _x, float _y, float[] _status, GameObject _record) : base()
    {
        TaskType = _taskType;
        TaskID = 0;
        Crdn[0] = _x;
        Crdn[1] = _y;

        Point = _status[0];
        Time = _status[1];
        Current = _status[2];
        Press = _status[3];

        TaskObject = GameManager.Instance.objectsManager.InstantiateObject(_taskType, TaskID, _x, _y, _record);
    }
}

class EraseTask{
    public EraseTask(GameObject _gameObject){
        string searchTag = "";
        if(_gameObject.tag == "NickelPoint"){
            searchTag = "Nickel";
            _gameObject = _gameObject.transform.parent.gameObject;
        }else{
            searchTag = _gameObject.tag;
        } 

        Debug.Log("Search Tag : " + searchTag);
        // 태그에 맞는 리스트가 있는지 확인
        if (GameManager.instance.taskDATA.taskListMap.TryGetValue(searchTag, out List<TaskUnit> taskList)) {
            Debug.Log("TaskList Found : " + searchTag);
            // 해당 오브젝트의 콜라이더를 저장(픽셀 콜라이더)
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




