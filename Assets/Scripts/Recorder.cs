using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Recorder : MonoBehaviour
{
    public int animationTake;
    public string animationName;
    public string dataPath;
    //public TMP_Text dataLocation;
    public GameObject[] objectsToRecord;
    public int currentFrame = 0;
    public bool startRecording;
    public bool endRecording;
    private SaveFile save;

    public GameObject mirror;
    public GameObject playback;

    public PersonalDebugConsole debug;

    private void Start()
    {
        InitialiseValues();        
    }
    public void InitialiseValues()
    {

        if (string.IsNullOrEmpty(animationName))
        {
            animationName = "Stock Animation";
        }
        dataPath = Application.persistentDataPath + "/FILES/";
        Debug.Log(dataPath);
        save = new SaveFile();
        save.positions = new List<VectorList>();
        save.rotations = new List<QuaternionList>();
        currentFrame = 0;

        for (int i = 0; i < objectsToRecord.Length; i++)
        {
            save.positions.Add(new VectorList());
            save.rotations.Add(new QuaternionList());
        }

        for (int i = 0; i < objectsToRecord.Length; i++)
        {
            save.positions[i].position = new List<Vector3>();
            save.rotations[i].rotation = new List<Quaternion>();
        }

    }

    private void Update()
    {
        //////if (Input.GetKeyDown(KeyCode.A))
        //////{
        //////    Debug.Log("Text file created successfully at: " + dataPath);
        //////    File.WriteAllText(dataPath, "Good morning kind sir, did this work?");
        //////}
        //dataLocation.text =  dataPath + "/ ______ Frame: " + currentFrame ;
        if (OVRInput.GetDown(OVRInput.RawButton.A, OVRInput.Controller.All))
        {
            debug.Log("Start Recording");
            startRecording = true;
        }
        if (startRecording)
        {
            if (objectsToRecord.Length > 0)
            {
                for (int i = 0; i < objectsToRecord.Length; i++)
                {                    
                    save.positions[i].position.Add(objectsToRecord[i].transform.position);
                    save.rotations[i].rotation.Add(objectsToRecord[i].transform.rotation);
                }
                currentFrame++;
            }
            if (OVRInput.GetDown(OVRInput.RawButton.X, OVRInput.Controller.All) || endRecording)
            {
                debug.Log("End Recording");
                startRecording = false;
                GetComponent<Movement>().enabled = true;
                //SaveRecordingAsText();
                Debug.Log(save.rotations[0].rotation[50]);
                PlayAnimation();
            }
        }        
    }

    public void PlayAnimation()
    {
        debug.Log("Attempting To Save Recording");
        mirror.SetActive(false);
        playback.GetComponent<AnimationPlayer>().save = save;

        JsonSaveFile jsonOutput = new JsonSaveFile();
        jsonOutput.values = new List<BuiltPositionsAndRotations>();
        for (int i = 0; i < save.positions.Count; i++)
        {
            jsonOutput.values.Add(new BuiltPositionsAndRotations());

            jsonOutput.values[i].xp = new List<float>();
            jsonOutput.values[i].yp = new List<float>();
            jsonOutput.values[i].zp = new List<float>();

            jsonOutput.values[i].xr = new List<float>();
            jsonOutput.values[i].yr = new List<float>();
            jsonOutput.values[i].zr = new List<float>();
            jsonOutput.values[i].wr = new List<float>();

            for (int j = 0; j < save.positions[i].position.Count; j++)
            {
                jsonOutput.values[i].xp.Add(save.positions[i].position[j].x);
                jsonOutput.values[i].yp.Add(save.positions[i].position[j].y);
                jsonOutput.values[i].zp.Add(save.positions[i].position[j].z);

                jsonOutput.values[i].xr.Add(save.rotations[i].rotation[j].x);
                jsonOutput.values[i].yr.Add(save.rotations[i].rotation[j].y);
                jsonOutput.values[i].zr.Add(save.rotations[i].rotation[j].z);
                jsonOutput.values[i].wr.Add(save.rotations[i].rotation[j].w);
            }
        }

        string output = JsonUtility.ToJson(jsonOutput, true);
        //string output = JsonUtility.ToJson(save);
        //Debug.Log(output);
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        File.WriteAllText(dataPath + animationName + " " + animationTake + ".txt", output);
        debug.Log("Saved Recording Successfully, Attempting to Playback Recording");

        playback.SetActive(true);
        gameObject.GetComponent<Recorder>().enabled = false;
        playback.GetComponent<AnimationPlayer>().enabled = true;
        playback.GetComponent<AnimationPlayer>().playOnStart = true;
        debug.Log("Playback Started Successfully");


    }

    public void SaveRecordingAsText()
    {
        JsonSaveFile jsonOutput = new JsonSaveFile();
        jsonOutput.values = new List<BuiltPositionsAndRotations>();
        for (int i = 0; i < save.positions.Count; i++)
        {
            jsonOutput.values.Add(new BuiltPositionsAndRotations());

            jsonOutput.values[i].xp = new List<float>();
            jsonOutput.values[i].yp = new List<float>();
            jsonOutput.values[i].zp = new List<float>();

            jsonOutput.values[i].xr = new List<float>();
            jsonOutput.values[i].yr = new List<float>();
            jsonOutput.values[i].zr = new List<float>();
            jsonOutput.values[i].wr = new List<float>();

            for (int j = 0; j < save.positions[i].position.Count; j++)
            {
                jsonOutput.values[i].xp.Add(save.positions[i].position[j].x);
                jsonOutput.values[i].yp.Add(save.positions[i].position[j].y);
                jsonOutput.values[i].zp.Add(save.positions[i].position[j].z);

                jsonOutput.values[i].xr.Add(save.rotations[i].rotation[j].x);
                jsonOutput.values[i].yr.Add(save.rotations[i].rotation[j].y);
                jsonOutput.values[i].zr.Add(save.rotations[i].rotation[j].z);
                jsonOutput.values[i].wr.Add(save.rotations[i].rotation[j].w);
            }
        }

        string output = JsonUtility.ToJson(jsonOutput,true);
        //if (Directory.Exists(dataPath))
        //{
        //    if (!File.Exists(dataPath))
        //    {
        //        File.Create(dataPath);
        //    }
        //    File.WriteAllText(dataPath + "/Animation Output.txt", output);
        //}
        //else
        //{
        //    Directory.CreateDirectory(dataPath);

        //    //File.Create(Application.dataPath + "./Data/Animation Output.txt");

        //    File.WriteAllText(dataPath + "/Animation Output.txt", output);
        //}
        //gameObject.GetComponent<Recorder>().enabled = false;
        File.WriteAllText(dataPath, output);
        Debug.Log("Saved Data Successfully");
        mirror.SetActive(false);
        playback.GetComponent<AnimationPlayer>().mainAnimation = output;
        playback.SetActive(true);
        gameObject.GetComponent<Recorder>().enabled = false;

    }

}

[System.Serializable]
public class JsonSaveFile
{
    public List<BuiltPositionsAndRotations> values;    
}

[System.Serializable]
public class BuiltPositionsAndRotations
{
    public List<float> xp;
    public List<float> yp;
    public List<float> zp;

    public List<float> xr;
    public List<float> yr;
    public List<float> zr;
    public List<float> wr;
}

