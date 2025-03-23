using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveWithMouse : MonoBehaviour
{
    [SerializeField] private GameObject _camera;

    [Tooltip("드래그 하여 이동할 거리 비율")]
    [SerializeField] private float _moveRate;

    [SerializeField] private RectTransform m_ViewportRect;


    public float zoomSpeed;
    public float minDistance;
    public float maxDistance;
    public bool CursorOnUI;


    public float xValue;

    public float yValue;



    private Vector3 _tmpClickPos;

    private Vector3 _tempCameraPos;

    private Camera cameraComponent;

    // void OnEnable()
    // {
    //     m_ViewportRect.(0.25f, 0.25f, 0.75f, 0.75f);
    // }


    private void Start()
    {
        cameraComponent = GetComponent<Camera>();
    }



    private void Update()
    {
        UIDetect();
        MouseMovement();
        WheelMovement();
        //WheelMovement2();
    }

    //마우스가 UI위에 있는지 감지하는 함수
    void UIDetect(){
        CursorOnUI = EventSystem.current.IsPointerOverGameObject();
    }

    /// <summary>마우스로 카메라를 이동시키는 함수</summary>
    private void MouseMovement()
    {
        if(CursorOnUI || GameManager.instance.uiManager.Moveable == false){
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //좌클릭시 카메라, 클릭 위치 값을 저장해둔다.
            //좌클릭을 꾹 누르고 있을 때 위치를 이동시키기 위함
            _tmpClickPos = Input.mousePosition;
            _tempCameraPos = _camera.transform.position;
        }

        else if (Input.GetMouseButton(0))
        {
            //(저장한 클릭 위치 - 현재 마우스 위치)로 이동할 방향 및 거리를 계산한다.
            Vector3 movePos = Camera.main.ScreenToViewportPoint(_tmpClickPos - Input.mousePosition);
            Vector3 nextMove = _tempCameraPos + (movePos * _moveRate);
            if(nextMove.x > xValue || nextMove.x < (-1 * xValue) || nextMove.y > yValue || nextMove.y < (-1 * yValue) ){
                return;
            }else{
                _camera.transform.position = _tempCameraPos + (movePos * _moveRate);
            }
        }
    }

    //마우스 휠로 확대 축소하는 함수
    private void WheelMovement(){
        if(CursorOnUI || GameManager.instance.uiManager.Moveable == false){
            return;
        }
        float zoomAmout = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * -1;
        float distance = _camera.transform.position.z - zoomAmout;

        // 디버그 로그 추가
        Debug.Log("Mouse ScrollWheel value: " + Input.GetAxis("Mouse ScrollWheel"));


        distance = Mathf.Clamp(distance, maxDistance, minDistance);

        _camera.transform.position = new Vector3(_camera.transform.position.x, _camera.transform.position.y, distance);

    }
}
