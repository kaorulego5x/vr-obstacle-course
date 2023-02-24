using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using UnityEngine.SceneManagement;


public class VRInputModule : BaseInputModule
{
    public Camera camera;
    public SteamVR_Input_Sources LeftTargetSource;
    public SteamVR_Input_Sources RightTargetSource;
    public SteamVR_Action_Boolean ClickAction;
    public GameObject leftController = null;
    public GameObject rightController = null;

    private GameObject CurrentObject = null;
    private PointerEventData Data = null;

    private void Start() {
    }

    protected override void Awake()
    {
        base.Awake();

        Data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        Data.Reset();
        Data.position = new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2);
        //RaycastResult RaycastResultCache;
        eventSystem.RaycastAll(Data, m_RaycastResultCache);
        Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        CurrentObject = Data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(Data, CurrentObject);
        if(ClickAction.GetStateDown(LeftTargetSource))
            ProcessPress(Data);

        if(ClickAction.GetStateUp(LeftTargetSource))
            ProcessPress(Data);

        if(ClickAction.GetStateDown(RightTargetSource))
            ProcessPress(Data);

        if(ClickAction.GetStateUp(RightTargetSource))
            ProcessPress(Data);

    }

    public PointerEventData GetData()
    {
        return Data;
    }

    private void handleRayCastHit(GameObject hitObject) {
        if (hitObject.CompareTag("Obstacle")) {
            Destroy(hitObject);
        } else if (hitObject.CompareTag("Start")) {
            SceneManager.LoadScene("Hit");
        }
    }

    private void ProcessPress(PointerEventData data)
    {
        Data.pointerPressRaycast = data.pointerCurrentRaycast;
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(CurrentObject, Data, ExecuteEvents.pointerDownHandler);

        RaycastHit hit;

        if(newPointerPress == null){
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentObject);

            Data.pressPosition = data.position;
            Data.pointerPress = newPointerPress;
            Data.rawPointerPress = CurrentObject;
            
            if (Physics.Raycast(leftController.transform.position, leftController.transform.forward, out hit, 15.0f)) {
                Debug.Log("left clicked");
                Debug.Log(hit.transform.gameObject);
                handleRayCastHit(hit.transform.gameObject);
            }

            if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 15.0f)) {
                handleRayCastHit(hit.transform.gameObject);
            }
        }
    }

    private void ProcessRelease(PointerEventData data)
    {
        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(CurrentObject);

        if(Data.pointerPress == pointerUpHandler){
            ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerClickHandler);
        }

        eventSystem.SetSelectedGameObject(null);

        Data.pressPosition = Vector2.zero;
        Data.pointerPress = null;
        Data.rawPointerPress = null;


    }





}
