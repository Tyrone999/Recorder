using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimationNameDisplayer : MonoBehaviour
{

    public TMP_Text animationName;

    void Update()
    {
        animationName.text = StaticVariables.animationName;
    }
}
