using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelector : MonoBehaviour
{
    public Material defaultColor;
    public Material colorIfInteractable;

    public bool colorState;

    void Update()
    {
        if (colorState)
        {
            GetComponent<LineRenderer>().material = defaultColor;
        }
        else
        {
            GetComponent<LineRenderer>().material = colorIfInteractable;
        }
    }
}
