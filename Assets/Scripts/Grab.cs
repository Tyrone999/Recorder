using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

    public bool grab = false;
    public bool hold = false;
    public GameObject placeholder;
    public bool runOnce;
    public Transform activeHand;

    private GameObject hand;

    private void Start()
    {
        runOnce = true;
        if (runOnce)
        {
            gameObject.transform.position = placeholder.transform.position;
            runOnce = false;
        }
    }

    private void Update()
    {
        if (grab)
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger,OVRInput.Controller.All))
            {
                hold = true;
            }
            else
            {
                hold = false;
            }
            if (hold)
            {

                gameObject.transform.position = activeHand.position;
                runOnce = true;
            }
            else
            {
                if (runOnce)
                {
                    gameObject.transform.position = placeholder.transform.position;
                    runOnce = false;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!hold)
        {
            if (other.CompareTag("Controller"))
            {
                grab = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            grab = false;
            hold = false;
        }   
    }
}
