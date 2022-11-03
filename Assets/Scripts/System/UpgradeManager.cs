using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{   
    public static UpgradeManager instance;
    private string file = "upgrade.txt";
    public Stats data;

    private void Awake()
    {
         if (instance == null) {
         instance = this;
         DontDestroyOnLoad(gameObject);
        } else {
         Object.Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(data);
        WriteToFile(file, json);
    }

    public void Load()
    {
        data = new Stats();
        string json = ReadFromFile(file);
        JsonUtility.FromJsonOverwrite(json, data);
    }

    private void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream FileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(FileStream))
        {
            writer.Write(json);
        }
    }

    private string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else 
        {   
            Stats newStats = new Stats();
            string json = JsonUtility.ToJson(newStats);
            // Write a default stats
            WriteToFile(file, json);
            return json;
        }
    }

    private string GetFilePath(string fileName) 
    {
        return Application.persistentDataPath + "/" + fileName;
    }
    
}
