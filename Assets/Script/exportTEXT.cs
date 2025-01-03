using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class exportTEXT : MonoBehaviour
{

    string path = "Assets/ExportData/";
    string fileNmae = "";
    StreamWriter sw;

    public void Exporttxt(string _fileName){
        int i = 0;
        while(File.Exists(path)){
            if(false == File.Exists(path + _fileName)){
                sw = new StreamWriter(path + _fileName);
                sw.WriteLine("쓰고 싶은 내용");
                sw.Flush();
                sw.Close();
                break;
            }else{
                i++;
            }
        }
        
    }
}
