using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OVRSimpleJSON;
using UnityEngine.UI;
using System.Linq;

public class AnimationPlayer : MonoBehaviour
{
    public TMP_Text frame;
    public GameObject[] objectsToMove;
    public int currentFrame = 0;
    public int maxFrame = 0;
    public bool playAnimation;
    public bool endAnimation;
    public bool playOnStart;
    public TextAsset mainAnimation;
    private SaveFile load;

    private void Start()
    {
        InitialiseValues();
    }
    public void InitialiseValues()
    {
        load = new SaveFile();
        load.positions = new List<VectorList>();
        load.rotations = new List<QuaternionList>();
        currentFrame = 0;
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            load.positions.Add(new VectorList());
            load.rotations.Add(new QuaternionList());
        }
        for (int i = 0; i < objectsToMove.Length; i++)
        {
            load.positions[i].position = new List<Vector3>();
            load.rotations[i].rotation = new List<Quaternion>();
        }
        if (mainAnimation != null)
        {
            PlayRecordingfromText();
        }
        else
        {
            Debug.LogWarning("There is no animation text file in the 'Main Animation' variable");
        }
        if (playOnStart)
        {
            playAnimation = true;
        }
    }
    private void Update()
    {

        if (mainAnimation != null)
        {
            if (playAnimation && !endAnimation)
            {
                PlayAnimation();
            }
        }
        else
        {
            playAnimation = false;
        }
    }

    public void PlayAnimation()
    {
        if (currentFrame < maxFrame)
        {
            for (int i = 0; i < objectsToMove.Length; i++)
            {
                objectsToMove[i].transform.position = load.positions[i].position[currentFrame];
                objectsToMove[i].transform.rotation = load.rotations[i].rotation[currentFrame];
            }
            currentFrame++;
        }
        else
        {
            playAnimation = false;
            endAnimation = true;
            currentFrame = 0;
        }
    }
    public void PlayRecordingfromText()
    {
        string anim = mainAnimation.text;
        JsonSaveFile input = JsonConvert.DeserializeObject<JsonSaveFile>(anim);
        gameObject.GetComponent<Recorder>().enabled = false;
        for (int i = 0; i < input.values.Count; i++)
        {
            //load.positions[i] = new VectorList();
            //load.rotations[i] = new QuaternionList();

            load.positions[i].position = new List<Vector3>();
            load.rotations[i].rotation = new List<Quaternion>();

            for (int j = 0; j < input.values[i].xp.Count; j++)
            {
                load.positions[i].position.Add(new Vector3(input.values[i].xp[j], input.values[i].yp[j], input.values[i].zp[j]));

                load.rotations[i].rotation.Add(new Quaternion(input.values[i].xr[j], input.values[i].yr[j], input.values[i].zr[j], input.values[i].wr[j]));
            }
        }
        maxFrame = load.positions[0].position.Count;
        Application.Quit();
    }
}
