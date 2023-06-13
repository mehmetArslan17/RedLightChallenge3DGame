using System.IO;
using UnityEngine;

public class SaveManager
{
    public static void SaveData(DataScripts gameData)
    {
        var json = JsonUtility.ToJson(gameData);
        File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDatas.txt", json);
    }

    public static void LoadData(DataScripts gameData)
    {
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDatas.txt"))
        {
            var json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDatas.txt");
            JsonUtility.FromJsonOverwrite(json, gameData);
        }
        else
        {
            
            var json = JsonUtility.ToJson(gameData);
            File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "GameDatas.txt", json);
        }
    }
}
