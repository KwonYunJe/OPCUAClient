using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Property property;  
    public Task task;
    public TaskDATA taskDATA;
    public ObjectsManager objectsManager;
    public InputManager inputManager;
    public UIManager uiManager;

    private void Awake() {
        InstantiateGM();
        property = new Property();
        task = new Task();
        taskDATA = new TaskDATA();
    }

    //GM 인스턴스 생성
    private void InstantiateGM(){
        if(instance == null){
            instance = this;
        }else{
            Debug.LogWarning("There is more than one GameManager instance");
            Destroy(gameObject);
        }
    }

    public static GameManager Instance{
        get{
            if (instance == null){
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if(instance == null){
                    Debug.LogError("There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }
            return instance;
        }
    }

    private void Start() {
        //게임이 시작되면 property를 초기화한다.
        property.InitProperty();
    }
}

//프로퍼티 클래스
public class Property
{
    public enum TaskType{Holder, Pin, Cell, Nickel, Welding}
    public TaskType taskType;
    public enum PinType{Pin1}
    public PinType pinType;
    public enum HolderType{Holder2x1, Holder1x2, Holder3x1, Holder1x3, Holder5x5}
    public HolderType holderType;
    public enum CellType{Plus_M50, Minus_M50, Pluse_50E, Minus_50E, Pluse_40T, Minus_40T}
    public CellType cellType;
    public enum NickelType{Nickel_1, Nickel_2, Nickel_3, Nickel_4}
    public NickelType nickelType;
    public Action<TaskType> ChangeTask;    //모드 변경 이벤트
    
    //프로퍼티 초기화
    public void InitProperty(){
        taskType = TaskType.Holder;
        holderType = HolderType.Holder2x1;
        cellType = CellType.Plus_M50;
        nickelType = NickelType.Nickel_1;
    }

    public void SetProperty(int _taskID){
        //프로퍼티 설정
        switch(taskType){
            case TaskType.Holder:
                if(_taskID >= HolderType.GetValues(typeof(HolderType)).Length){
                    Debug.Log("There is no such holder type");
                    return;
                }else{
                    holderType = (HolderType)_taskID;
                }
                break;
            case TaskType.Pin:

                break;
            case TaskType.Cell:
                if(_taskID >= CellType.GetValues(typeof(CellType)).Length){
                    Debug.Log("There is no such cell type");
                    return;
                }else{
                    cellType = (CellType)_taskID;
                }
                break;
            case TaskType.Nickel:
                if(_taskID >= NickelType.GetValues(typeof(NickelType)).Length){
                    Debug.Log("There is no such nickel type");
                    return;
                }else{
                    nickelType = (NickelType)_taskID;
                }
                break;
            case TaskType.Welding:
                break;
            default:
                Debug.LogError("There is no such task type");
                break;
        }
    }

    public void ChangeTaskType(){
        taskType = (TaskType)(((int)taskType + 1) % System.Enum.GetValues(typeof(TaskType)).Length);
        //작업 모드 변경 이벤트 발생
        ChangeTask?.Invoke(taskType);
    }

    public Task GetTask(){
        return GameManager.instance.task;
    }
    public ObjectsManager GetObjectsManager(){
        return GameManager.instance.objectsManager;
    }

    //현재 작업 타입에 맞게 작업 타입 아이디를 반환
    public int GetNowTaskTypeID(){
        switch(taskType){
            case TaskType.Holder:
                return (int)holderType;
            case TaskType.Pin:
                return (int)pinType;
            case TaskType.Cell:
                return (int)cellType;
            case TaskType.Nickel:
                return (int)nickelType;
            case TaskType.Welding:
                return 0;
            default:
                return 0;
        }
    }
}


