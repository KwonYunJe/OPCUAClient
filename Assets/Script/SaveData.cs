using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//해당 스크립트는 공정 정보의 관리를 좀 더 일괄적인 측면에서 하기위해 작성함
//또한 AllScript 오브젝트에 component로 존재함
public class SaveData : MonoBehaviour
{

//홀더가 저장될 리스트(홀더의 스크립트)
    List<Holder> HolderProcess = new();
//홀더핀이 저장될 리스트(핀의 스크립트)
    public List<Pin> PinProcess = new();
//셀이 저장될 리스트(셀의 스크립트)
    public List<Cell> CellProcess = new();
//니켈이 저장될 리스트(니켈의 스크립트)
    public List<NickelLine> NickelProcess = new();
//용접이 저장될 리스트(용접포인트의 스크립트)
    public List<WeldingPixel> WeldingProcess = new();

/**************************************************************/
//홀더
//리스트에 생성된 홀더를 추가
    public void AddHolderProcess(Holder holder){
        //
        holder.num = HolderProcess.Count;
        HolderProcess.Add(holder);
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetHolderMaterialInfo();
    }
//마지막 홀더를 삭제
    public void DeleteLastIndex_Holder(){
        Holder delHolder = HolderProcess[HolderProcess.Count-1];
        HolderProcess.Remove(delHolder);
        delHolder.Destroy();
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetHolderMaterialInfo();
    }

    public Holder GetHolder(int _index){
        return HolderProcess[_index];
    }


//홀더의 총 개수를 반환
    public int GetHolderProcessCount(){
        return HolderProcess.Count;
    }

//홀더 종류별 개수 반환
    public int[] GetHolderCount(){

        int[] ints = new int[]{0,0,0,0,0};
        foreach(Holder holder in HolderProcess){
            switch(holder.HolderType){
                case Panel_Holder.Type.Two:
                    ints[0]++;
                break;
                case Panel_Holder.Type.TwoVertical:
                    ints[1]++;
                break;
                case Panel_Holder.Type.Three:
                    ints[2]++;
                break;
                case Panel_Holder.Type.ThreeVertical:
                    ints[3]++;
                break;
                case Panel_Holder.Type.Five:
                    ints[4]++;
                break;
            }
        }
        return ints;
    }

//홀더 배치 프로세스 순서 변경
    public void ChangeProcessHolder(int _index, int _type){
        //클릭된 버튼의 부모 오브젝트를 찾고 그 부모 오브젝트의 0번째 자식 오브젝트(순서를 저장)를 찾아 순번을 가져온다. 

        if(_type == 0){      //선택된 홀더 순서를 앞으로 한 칸 당김
            Holder selectedHolder = HolderProcess[_index];
            HolderProcess.Remove(selectedHolder);
            HolderProcess.Insert(_index-1, selectedHolder);
        }else{                  //선택된 홀더 순서를 뒤로 한 칸 밈
            Holder selectedHolder = HolderProcess[_index];
            HolderProcess.Remove(selectedHolder);
            HolderProcess.Insert(_index+1, selectedHolder);
        }

    }

//홀더 배치 프리뷰 재생 
    public void AllInactiveHolder(){
        Debug.Log("홀더 오브젝트 비활성화 시작됨");
        foreach(Holder holder in HolderProcess){
            Debug.Log("비활성화 대상 : " + holder);
            holder.InactiveObject();
        }
        StartCoroutine(StartActiveAllHolder(0));
    }

    IEnumerator StartActiveAllHolder(int _index){
        Debug.Log("코루틴 시작, 현재 인덱스 : " + _index);
        yield return new WaitForSeconds(0.5f);
        HolderProcess[_index].ActiveObject();
        Debug.Log(HolderProcess[_index] + "의 상태 : " + HolderProcess[_index].gameObject.activeSelf);
        if(_index >= HolderProcess.Count-1){
            Debug.Log("코루틴 종료");
            StopCoroutine("StartActiveAllHolder");
        }else{
            Debug.Log("다음 코루틴 호출, 다음 인덱스 : " + _index++);
            StartCoroutine(StartActiveAllHolder(_index++));
        }
    }
/**************************************************************/
//핀
    public void AddPinProcess(Pin _pin){
        PinProcess.Add(_pin);
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetPinMaterialInfo();
    }

    public void DeleteLastIndex_Pin(){
        Pin delPin = PinProcess[PinProcess.Count-1];
        PinProcess.Remove(delPin);
        delPin.Reset();
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetPinMaterialInfo();
    }

    public int GetPinProcessCount(){
        return PinProcess.Count;
    }

    public void ChangeProcessPin(int _index, int _type){
        //클릭된 버튼의 부모 오브젝트를 찾고 그 부모 오브젝트의 0번째 자식 오브젝트(순서를 저장)를 찾아 순번을 가져온다. 

        if(_type == 0){      //선택된 핀 순서를 앞으로 한 칸 당김
            Pin selectedPin = PinProcess[_index];
            PinProcess.Remove(selectedPin);
            PinProcess.Insert(_index-1, selectedPin);
        }else{                  //선택된 핀 순서를 뒤로 한 칸 밈
            Pin selectedPin = PinProcess[_index];
            PinProcess.Remove(selectedPin);
            PinProcess.Insert(_index+1, selectedPin);
        }
    }

    public void ResetProcessPin(){
        while(PinProcess.Count > 0){
            DeleteLastIndex_Pin();
        }
    }

    public void AllInactivePin(){
        Debug.Log("핀 오브젝트 비활성화 시작됨");
        foreach(Pin _pin in PinProcess){
            Debug.Log("비활성화 대상 : " + _pin);
            _pin.InactiveObject();
        }
        StartCoroutine(StartActiveAllPin(0));
    }

        IEnumerator StartActiveAllPin(int _index){
        Debug.Log("코루틴 시작, 현재 인덱스 : " + _index);
        yield return new WaitForSeconds(0.5f);
        PinProcess[_index].ActiveObject();
        Debug.Log(PinProcess[_index] + "의 상태 : " + PinProcess[_index].gameObject.activeSelf);
        if(_index >= PinProcess.Count-1){
            Debug.Log("코루틴 종료");
            StopCoroutine("StartActiveAllPin");
        }else{
            Debug.Log("다음 코루틴 호출, 다음 인덱스 : " + _index++);
            StartCoroutine(StartActiveAllPin(_index++));
        }
    }

    /**************************************************************************************/

    //셀 프로세스를 추가
    public void AddCellProcess(Cell _cell){
        CellProcess.Add(_cell);
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetCellMaterialInfo();
    }
    //셀 프로세스를 맨 뒤 한 개만 삭제
    public void DeleteLastIndex_Cell(){
        Cell delCell = CellProcess[CellProcess.Count-1];
        CellProcess.Remove(delCell);
        delCell.Destroy();
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetCellMaterialInfo();
    }
    public void ClearCellProcess(){
        while(CellProcess.Count>0){
            DeleteLastIndex_Cell();
        }
    }
    //셀 프로세스의 총 개수를 반환
    public int GetCellProcessCount(){
        return CellProcess.Count;
    }

    public void ChangeProcessCell(int _index, int _type){
        //클릭된 버튼의 부모 오브젝트를 찾고 그 부모 오브젝트의 0번째 자식 오브젝트(순서를 저장)를 찾아 순번을 가져온다. 
        if(_type == 0){      //선택된 셀 순서를 앞으로 한 칸 당김
            Cell selectedCell = CellProcess[_index];
            CellProcess.Remove(selectedCell);
            CellProcess.Insert(_index-1, selectedCell);
        }else{                  //선택된 셀 순서를 뒤로 한 칸 밈
            Cell selectedCell = CellProcess[_index];
            CellProcess.Remove(selectedCell);
            CellProcess.Insert(_index+1, selectedCell);
        }
    }

    //셀 프로세스의 셀 종류별 개수를 반환
    public int[] GetCellCount(){
        int[] ints = new int[]{0,0,0,0,0,0};

        foreach(Cell _cell in CellProcess){
            switch(_cell.CellType){
                case Panel_Cell.Type.plus50E:
                ints[0]++;
                break;
                case Panel_Cell.Type.minus50E:
                ints[1]++;
                break;
                case Panel_Cell.Type.plusM50:
                ints[2]++;
                break;
                case Panel_Cell.Type.minusM50:
                ints[3]++;
                break;
                case Panel_Cell.Type.plus40T:
                ints[4]++;
                break;
                case Panel_Cell.Type.minus40T:
                ints[5]++;
                break;
            }
        }
        return ints;
    }

    public void AllInactiveCell(){
        Debug.Log("홀더 오브젝트 비활성화 시작됨");
        foreach(Cell _cell in CellProcess){
            Debug.Log("비활성화 대상 : " + _cell);
            _cell.InactiveObject();
        }
        StartCoroutine(StartActiveAllCell(0));
    }

    IEnumerator StartActiveAllCell(int _index){
        Debug.Log("코루틴 시작, 현재 인덱스 : " + _index);
        yield return new WaitForSeconds(0.5f);
        CellProcess[_index].ActiveObject();
        Debug.Log(CellProcess[_index] + "의 상태 : " + CellProcess[_index].gameObject.activeSelf);
        if(_index >= CellProcess.Count-1){
            Debug.Log("코루틴 종료");
            StopCoroutine("StartActiveAllCell");
        }else{
            Debug.Log("다음 코루틴 호출, 다음 인덱스 : " + _index++);
            StartCoroutine(StartActiveAllCell(_index++));
        }
    }

/**********************************************************************************************/
    public void AddNickelProcess(NickelLine _NickelLine){
        NickelProcess.Add(_NickelLine);
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetNickelMaterialInfo();
    }

    public void DeleteLastIndex_Nickel(){
        NickelLine delNickelLine = NickelProcess[NickelProcess.Count-1];
        NickelProcess.Remove(delNickelLine);
        delNickelLine.DeleteProcess();
        gameObject.GetComponent<AllScript>().UI.GetComponent<UIController>().SetNickelMaterialInfo();
    }

    public int[] GetNickelCount(){
        int[] ints = new int[]{0,0,0,0};
        
        foreach(NickelLine _nickelLine in NickelProcess){
            switch(_nickelLine.NickelType){
                case Panel_Nickel.Type.Line1:
                    ints[0]++;
                break;
                case Panel_Nickel.Type.Line2:
                    ints[1]++;
                break;
                case Panel_Nickel.Type.Line3:
                    ints[2]++;
                break;
                case Panel_Nickel.Type.Line4:
                    ints[3]++;
                break;
            }
        }

        return ints;
    }

    public int GetNickelProcessCount(){
        return NickelProcess.Count;
    }

    public void ChangeProcessNickel(int _index, int _type){
        //클릭된 버튼의 부모 오브젝트를 찾고 그 부모 오브젝트의 0번째 자식 오브젝트(순서를 저장)를 찾아 순번을 가져온다. 

        if(_type == 0){      //선택된 니켈 순서를 앞으로 한 칸 당김
            NickelLine selectedNickelLine = NickelProcess[_index];
            NickelProcess.Remove(selectedNickelLine);
            NickelProcess.Insert(_index-1, selectedNickelLine);
        }else{                  //선택된 니켈 순서를 뒤로 한 칸 밈
            NickelLine selectedNickelLine = NickelProcess[_index];
            NickelProcess.Remove(selectedNickelLine);
            NickelProcess.Insert(_index+1, selectedNickelLine);
        }
    }

    public void AllInactiveNickel(){
        foreach(NickelLine _nickelLine in NickelProcess){
            _nickelLine.InactiveObject();
        }
        StartCoroutine(StartActiveAllNickel(0));
    }

    IEnumerator StartActiveAllNickel(int _index){
        yield return new WaitForSeconds(0.5f);
        NickelProcess[_index].ActiveObject();
        if(_index >= NickelProcess.Count-1){
            Debug.Log("코루틴 종료");
            StopCoroutine("StartActiveAllNickel");
        }else{
            Debug.Log("다음 코루틴 호출, 다음 인덱스 : " + _index++);
            StartCoroutine(StartActiveAllNickel(_index++));
        }
    }
    /**********************************************************************************************/

    public void AddWeldingProcess(WeldingPixel _weldingPixel){
        if(WeldingProcess.Contains(_weldingPixel)){
            WeldingProcess.Remove(_weldingPixel);
        }
        WeldingProcess.Add(_weldingPixel);
        SortProcess();

        RefreshRecordLine();
    }

    public void DeleteLastIndex_Welding(){
        WeldingPixel delWelding = WeldingProcess[WeldingProcess.Count-1];
        WeldingProcess.Remove(delWelding);
        delWelding.Init();

        RefreshRecordLine();
    }

    public int GetWeldingProcessCount(){
        return WeldingProcess.Count;
    }
    public void ChangeProcessWelding(int _index, int _type){
        if(_type == 0){      //선택된 셀 순서를 앞으로 한 칸 당김
            WeldingPixel weldingPixel = WeldingProcess[_index];
            WeldingProcess.Remove(weldingPixel);
            WeldingProcess.Insert(_index-1, weldingPixel);
        }else{                  //선택된 셀 순서를 뒤로 한 칸 밈
            WeldingPixel weldingPixel = WeldingProcess[_index];
            WeldingProcess.Remove(weldingPixel);
            WeldingProcess.Insert(_index+1, weldingPixel);
        }
    }

    public void SortProcess(){
        var sortList = WeldingProcess.OrderBy(y => y.gameObject.transform.position.y).ThenBy(x => x.gameObject.transform.position.x).ToList();
        WeldingProcess = sortList;
    }

    public void AllInactiveWelding(){
        foreach(WeldingPixel _weldingPixel in WeldingProcess){
            _weldingPixel.InactiveObject();
        }
        StartCoroutine(StartActiveAllWelding(0));
    }

    public void RefreshRecordLine(){
        gameObject.GetComponent<AllScript>().GetUIController().RemoveRecordLineForWelding();
        int i = 1;
        foreach(WeldingPixel _weldingPixel in WeldingProcess){
            gameObject.GetComponent<AllScript>().GetUIController().AddRecordLineWelding(i++ ,_weldingPixel);
        }
    }

    IEnumerator StartActiveAllWelding(int _index){
        yield return new WaitForSeconds(0.5f);
        WeldingProcess[_index].ReActiveObject();
        if(_index >= WeldingProcess.Count-1){
            Debug.Log("코루틴 종료");
            StopCoroutine("StartActiveAllWelding");
        }else{
            Debug.Log("다음 코루틴 호출, 다음 인덱스 : " + _index++);
            StartCoroutine(StartActiveAllWelding(_index++));
        }
    }
}
