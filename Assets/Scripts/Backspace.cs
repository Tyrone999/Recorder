using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backspace : MonoBehaviour
{
    private bool hit = false;
    private bool canbehitagain = true;
    private bool resetKey = false;

    private void Update()
    {
        if (hit)
        {
            gameObject.transform.localPosition += new Vector3(0, -0.1f, 0);
        }
        if (hit && gameObject.transform.localPosition.y <= -0.5)
        {
            AddNewLetter();
            resetKey = true;
            hit = false;
        }
        if (resetKey)
        {
            gameObject.transform.localPosition += new Vector3(0, +0.1f, 0);
        }
        if (resetKey && gameObject.transform.localPosition.y >= 0)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, gameObject.transform.localPosition.z);
            canbehitagain = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TypingStick"))
        {
            if (canbehitagain)
            {
                hit = true;
                canbehitagain = false;
                resetKey = false;
            }
        }
    }

    public void AddNewLetter()
    {
        if (StaticVariables.animationName.Length > 0)
        {
            StaticVariables.animationName = StaticVariables.animationName.Substring(0, StaticVariables.animationName.Length - 1);
        }
    }
}
