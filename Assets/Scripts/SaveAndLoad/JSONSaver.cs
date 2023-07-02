using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONSaver
{
    public static JSONSaver Instance;

    private static PlayerData _playerData;
    private static string _path = "";
    private static string _persistentPath = "";

    static JSONSaver()
    {
        Instance = new JSONSaver();
        // FIXME: for testing only
        _playerData = new PlayerData(xp: 0, level: 4);
        _path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
        // TODO: change Project Settings > Player > Company Name
        _persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }

    public void SaveData()
    {
        string savePath = _persistentPath;

        Debug.Log("Saving Data at: " + savePath);
        string json = JsonUtility.ToJson(_playerData);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public void LoadData()
    {
        using StreamReader reader = new StreamReader(_persistentPath);
        string json = reader.ReadToEnd();

        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log(data.ToString());
    }
}
