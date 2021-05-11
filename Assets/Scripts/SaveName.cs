using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveName : MonoBehaviour
{
    private bool hit = false;
    private bool canbehitagain = true;
    private bool resetKey = false;

    public Recorder recorder;
    public GameObject bucket;
    public GameObject keyboard;
    public GameObject typingStick1;
    public GameObject typingStick2;

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
            StaticVariables.animationTake++;
            recorder.animationTake = StaticVariables.animationTake;
            recorder.animationName = StaticVariables.animationName;
            recorder.enabled = true;
            keyboard.SetActive(false);
            typingStick1.SetActive(false);
            typingStick2.SetActive(false);
            bucket.SetActive(true);
        }
    }
}
