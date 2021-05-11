using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 7.5f * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x * speed * 0.25f, 0, OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y * speed * 0.25f);
        transform.Rotate(0, OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x * speed, 0);
    }
}
