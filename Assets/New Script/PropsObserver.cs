using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsSubject : Subject
{
    private PropsObserver _propsObserver;

    public Property.TaskType CurrenttaskType {get {return _taskType;}}

    [SerializeField]
    private Property.TaskType _taskType = Property.TaskType.Holder;

    private void Awake() {
        _propsObserver = GetComponent<PropsObserver>();
    }

    private void Start() {
        Notify();
    }

    private void OnEnable() {
        if(_propsObserver != null){
            Attach(_propsObserver);
        }
    }

    private void OnDisable() {
        if(_propsObserver != null){
            Detach(_propsObserver);
        }
    }

    public void ChangeTaskType(Property.TaskType taskType){
        _taskType = taskType;
        Notify();
    }


}

public class PropsObserver : Observer
{
    private Property.TaskType _CurrenttaskType;
    PropsSubject _propsSubject;

    public override void OnNotify(Subject subject)
    {
        if(!_propsSubject){
            _propsSubject = subject.GetComponent<PropsSubject>();
        }

        if(_propsSubject){
            _CurrenttaskType = _propsSubject.CurrenttaskType;
        }   
    }
}


