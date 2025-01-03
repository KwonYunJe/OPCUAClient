using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordLine : MonoBehaviour
{
    public Text NumText;
    public Text TypeText;
    public Text CoordinateText;

    public void SetNumText(String _text){
        NumText.text = _text;
    }

    public void SetTypeText(String _text){
        TypeText.text = _text;
    }

    public void SetCdnText(String _text){
        CoordinateText.text = _text;
    }
    public void DeleteLine(){
        Destroy(gameObject);
    }
}
