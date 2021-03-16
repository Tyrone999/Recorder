using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Recorder : MonoBehaviour
{
    public string dataPath;
    public TMP_Text dataLocation;
    public GameObject[] objectsToRecord;
    public int currentFrame = 0;
    public bool startRecording;
    public bool endRecording;
    private SaveFile save;

    private void Start()
    {
        InitialiseValues();        
    }
    public void InitialiseValues()
    {
        dataPath = Application.persistentDataPath + "./Data";
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
        dataLocation.text =  dataPath + "/ ______ Frame: " + currentFrame ;
        if (OVRInput.Get(OVRInput.RawButton.A, OVRInput.Controller.All))
        {
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
            if (OVRInput.Get(OVRInput.RawButton.X, OVRInput.Controller.All) || endRecording)
            {               
                //startRecording = false;
                SaveRecordingAsText();
            }
        }        
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

        string output = JsonConvert.SerializeObject(jsonOutput as JsonSaveFile);
        if (Directory.Exists(dataPath))
        {
            if (!File.Exists(dataPath))
            {
                File.Create(dataPath);
            }
            File.WriteAllText(dataPath + "/Animation Output.txt", output);
        }
        else
        {
            Directory.CreateDirectory(dataPath);

            //File.Create(Application.dataPath + "./Data/Animation Output.txt");

            File.WriteAllText(dataPath + "/Animation Output.txt", output);
        }
        gameObject.GetComponent<Recorder>().enabled = false;
        Application.Quit();
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

