using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_RIG : MonoBehaviour
{
    public Transform controllerL;
    public Transform controllerR;

    void Start()
    {

    }

    void Update()
    {
        Debug.DrawRay(controllerL.position, controllerL.forward);
        Debug.DrawRay(controllerR.position, controllerR.forward);
        ControllerTracking();
        
    }

    void ControllerTracking()
    {
        controllerL.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        controllerR.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        controllerL.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        controllerR.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }

}
