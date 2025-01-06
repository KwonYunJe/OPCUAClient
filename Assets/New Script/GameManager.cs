using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Property property;  
    public Task task;
    public ObjectsManager objectsManager;

    private void Awake() {
        InstantiateGM();
        property = new Property();
        task = new Task();
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
        property.InitProperty();
    }
}

public class Property
{
    public enum TaskType{Holder, HolderPin, Cell, Nickel, Welding}
    public TaskType taskType;
    public enum HolderType{Holder2x1, Holder1x2, Holder3x1, Holder1x3, Holder5x5}
    public HolderType holderType;
    public enum CellType{Plus_M50, Minus_M50, Pluse_50E, Minus_50E, Pluse_40T, Minus_40T}
    public CellType cellType;
    public enum NickelType{Nickel_1, Nickel_2, Nickel_3, Nickel_4}
    public NickelType nickelType;
    
    public void InitProperty(){
        taskType = TaskType.Holder;
        holderType = HolderType.Holder2x1;
        cellType = CellType.Plus_M50;
        nickelType = NickelType.Nickel_1;
    }

    public Task GetTask(){
        return GameManager.instance.task;
    }
    public ObjectsManager GetObjectsManager(){
        return GameManager.instance.objectsManager;
    }
}
