using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPreview : MonoBehaviour
{
    public GameObject panel_Holder;
    public List<Holder> holer;

//play 오브젝트가 생성될 때 실행됨
    void Start () {
        panel_Holder = gameObject.GetComponent<AllScript>().PreviewPlayPanel;

    }
    

}
