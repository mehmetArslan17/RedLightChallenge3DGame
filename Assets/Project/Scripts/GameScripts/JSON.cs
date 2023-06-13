using System.IO;
using UnityEngine;

public static class JSON
{
    public static string directory = "/SaveData1/";
    public static string fileName = "data.json";

    public static void Save(DataScripts so)
    {
        string dir = Application.dataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        else
        {
            Debug.Log("Kayit olusmadi");
        }

        var json = JsonUtility.ToJson(so);
        File.WriteAllText(dir + fileName, json);
    }
    public static DataScripts Load(DataScripts data)
    {
        string fullPath = Application.dataPath + directory + fileName;

        if (File.Exists(fullPath))
        {
            var json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<DataScripts>(json);
        }
        else
        {
            Debug.Log("Kayýt Dosyasý Bulunamadý..");
        }
        return data;
    }
}