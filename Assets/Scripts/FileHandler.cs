using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class FileHandler
{
    // Convert list to json
    public static void SaveToJSON<T>(List<T> toSave, string filename)
    {
        Debug.Log(GetPath(filename));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        Writefile(GetPath(filename), content);
    }
    // Convert json to list
    public static List<T> ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPath(filename));
        if(string.IsNullOrEmpty(content) || content == "{}") 
        {
            return new List<T>();
        }
        List<T> res = JsonHelper.FromJson<T>(content).ToList();
        return res;
    }
    // Get the path to saved file
    private static string GetPath(string filename)
    {
        return Application.persistentDataPath + "/" +  filename;
    }
    // Save content to file
    private static void Writefile(string path, string content)
    {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using StreamWriter writer = new StreamWriter(fileStream);
        writer.Write(content);
    }
    // Read content from file
    private static string ReadFile(string path) 
    {
        if (File.Exists(path)) 
        {
            using StreamReader reader = new StreamReader(path);
            string content = reader.ReadToEnd();
            return content;
        }
        return "";
    }
}


// https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity/36244111#36244111https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity/36244111#36244111:~:text=2.%20MULTIPLE%20DATA(ARRAY%20JSON)
// Used to wrap arrays to json and from json
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
