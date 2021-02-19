using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedPointer : MonoBehaviour
{
    public GameObject remoteOffset;

    public Vector3 rotationOfRemoteOffset;

    public GameObject startPoint;
    public GameObject topPoint;
    public GameObject endPoint;

    public GameObject lineRenderer;

    private float currentPosition;

    public Vector3 startLine;
    public Vector3 endLine;
    public Vector3 curvedLinePoints;

    private Quaternion savedRotation;

    private OVRInput.RawAxis2D distanceInfluencerStick = OVRInput.RawAxis2D.LThumbstick;

    private Vector3[] linePoints = new Vector3[11];

    [Range(10, 20)]
    public int maximumTeleportDistance;

    public int heightPoint;
    public float distanceBetweenPoints;

    public Transform playerHandPosition;
    public Transform playerHandRotation;

    private void Start()
    {
        currentPosition = 0;
    }

    private void Update()
    {
        startPoint.transform.localPosition = playerHandPosition.transform.localPosition;
        playerHandRotation.transform.localRotation = playerHandPosition.transform.localRotation;
        rotationOfRemoteOffset = remoteOffset.transform.localRotation.eulerAngles;       
       
        startPoint.transform.localPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        startPoint.transform.localRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
        topPoint.transform.localPosition = new Vector3(startPoint.transform.localPosition.x, startPoint.transform.localPosition.y + heightPoint, startPoint.transform.localPosition.z + distanceBetweenPoints); //temporary to test bezier.
        endPoint.transform.localPosition = new Vector3(startPoint.transform.localPosition.x, topPoint.transform.localPosition.y - heightPoint, topPoint.transform.localPosition.z + distanceBetweenPoints);

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.2f)
        {
            lineRenderer.SetActive(true);
            for (int i = 0; i < 11; i++)
            {
                startLine = Vector3.Lerp(startPoint.transform.position, topPoint.transform.position, 0.1f * i);
                endLine = Vector3.Lerp(topPoint.transform.position, endPoint.transform.position, 0.1f * i);
                lineRenderer.GetComponent<LineRenderer>().SetPosition(i, Vector3.Lerp(startLine, endLine, 0.1f * i));
            }
            savedRotation = remoteOffset.transform.localRotation;
        }
        else
        {
            lineRenderer.SetActive(false);
        }
    }

}
