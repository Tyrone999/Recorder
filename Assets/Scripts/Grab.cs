using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{

    public bool grab = false;
    public bool hold = false;
    public GameObject placeholder;

    private GameObject hand;

    private void Update()
    {
       // if (grab)
        {
            if (OVRInput.Get(OVRInput.RawButton.RHandTrigger,OVRInput.Controller.All))
            {
                hold = true;
            }
            if (hold)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.transform.position = placeholder.transform.position;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            grab = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            grab = false;
        }   
    }
}
