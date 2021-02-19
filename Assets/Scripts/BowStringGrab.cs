using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowStringGrab : MonoBehaviour
{
    public Transform origionalPosition;
    public Transform handPosition;

    private Vector3 position;
    private bool getPosition = true;

    private float increment;

    private void Update()
    {
        if (!OVRInput.Get(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.RTouch))
        {
            if (getPosition)
            {
                position = gameObject.transform.position;
                getPosition = false;
            }
            else
            {
                if (increment <= 1)
                {
                    gameObject.transform.position = Vector3.Lerp(position, origionalPosition.position, increment);
                    increment += 0.1f;
                }
                if (increment > 1)
                {
                    gameObject.transform.position = origionalPosition.position;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("playerHand"))
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.RTouch))
            {
                gameObject.transform.position = handPosition.position;
                getPosition = true;
                increment = 0;
            }
        }
    }
}
