using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHandle : MonoBehaviour
{
    public GameObject HandObject;
    Vector3 OldRelativeHandPosition;

    private void Update()
    {
        if (HandObject != null)
        {
            transform.RotateAround(Vector3.zero,Vector3.down, angleDeltaDegrees(OldRelativeHandPosition, transform.InverseTransformPoint(HandObject.transform.position)));
            OldRelativeHandPosition = transform.InverseTransformPoint(HandObject.transform.position);
        }
    }
    float angleDeltaDegrees(Vector3 relativeHandPosition, Vector3 newRelativeHandPosition)
    {
        return (Mathf.Atan2(newRelativeHandPosition.z, newRelativeHandPosition.x) - Mathf.Atan2(relativeHandPosition.z, relativeHandPosition.x)) *Mathf.Rad2Deg; 
    }

    private void OnTriggerEnter(Collider other)
    {
        HandObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        HandObject = null;
    }
}
