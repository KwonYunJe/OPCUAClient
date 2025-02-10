using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer : MonoBehaviour
{
    public abstract void OnNotify(Subject subject);
}

public class Subject : MonoBehaviour
{
    private readonly ArrayList _observers = new ArrayList();
    
    public void Attach(Observer observer){
        _observers.Add(observer);
    }

    public void Detach(Object observer){
        _observers.Remove(observer);
    }

    public void Notify(){
        foreach(Observer observer in _observers){
            observer.OnNotify(this);
        }
    }
}
