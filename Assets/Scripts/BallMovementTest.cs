using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BallMovementTest : MonoBehaviour
{
    private InputDevice targetDevice;
    private Vector2 primary2DAxisValue;
    private bool applyMovement;

    public GameObject playerBall;
    public float speed;

    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics rightHand = InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right;
        InputDevices.GetDevicesWithCharacteristics(rightHand, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + " : " + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    void Update()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out primary2DAxisValue))
        {
            Vector2 min = new Vector2(-0.1f, -0.1f);
            Vector2 max = new Vector2(0.1f, 0.1f);
            if (primary2DAxisValue.magnitude >  max.magnitude || primary2DAxisValue.magnitude < min.magnitude)
            {
                applyMovement = true;
            }
            else
            {
                applyMovement = false;
                playerBall.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
    private void FixedUpdate()
    {
        if (applyMovement)
        {
            if (playerBall != null)
            {
                playerBall.GetComponent<Rigidbody>().velocity = new Vector3(primary2DAxisValue.x * speed * Time.deltaTime, 0, primary2DAxisValue.y * speed * Time.deltaTime);
            }
            else
            {
                Debug.Log("Player Ball Is Null!");
            }
        }
    }
}
