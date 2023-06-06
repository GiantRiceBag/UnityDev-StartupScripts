using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using System.Runtime.Serialization;

public enum SaveType
{
    Binary,
    Json
}

public class SaveManager : BSingleton<SaveManager>
{
    const int SIZE_MAX_SAVE = 1;
    const string PATH_SAVE = "/saves/";
    const string EXTENSION_SAVE_BINARY = ".save";
    const string EXTENSION_SAVE_JSON = ".json";

    List<string> saveFilesURL;
    List<string> SaveFilesURL { get { if (saveFilesURL is null) GetAllSaveFilesURL(); return saveFilesURL; } }
    int SaveFilesCount => SaveFilesURL.Count;

    System.Func<string,SaveType,string> NameToURL = (saveName,saveType) => Application.persistentDataPath + PATH_SAVE + saveName + (saveType == SaveType.Binary ? EXTENSION_SAVE_BINARY : EXTENSION_SAVE_JSON);
    System.Func<string, string> URLToName = (saveURL) => saveURL.Replace(SaveDirectory,"").Split('.')[0];
    static string SaveDirectory => Application.persistentDataPath + PATH_SAVE;

    public bool Save(string saveName,object data,SaveType saveType = SaveType.Binary)
    {
        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        if(SaveFilesCount >= SIZE_MAX_SAVE && !File.Exists(NameToURL(saveName,saveType)))
        {
            Debug.LogWarning($"Count of save files is out of MAXSIZE {SIZE_MAX_SAVE}");
            return false;
        }
        try
        {
            switch (saveType)
            {
                case SaveType.Binary:
                    {
                        BinaryFormatter formatter = GetBinaryFormatter();
                        FileStream stream = File.Create(NameToURL(saveName, saveType));
                        formatter.Serialize(stream, data);
                        stream.Close();
                        break;
                    }
                case SaveType.Json:
                    {
                        string json = JsonUtility.ToJson(data, true);
                        FileStream stream = File.Create(NameToURL(saveName, saveType));
                        StreamWriter writer = new StreamWriter(stream);
                        writer.Write(json);
                        writer.Close();
                        stream.Close();
                        break;
                    }
            }
            GetAllSaveFilesURL();
        }
        catch
        {
            return false;
        }
  
        return true;
    }

    public T LoadFromBinary<T>(string saveName)
    {
        try
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream stream = File.Open(NameToURL(saveName, SaveType.Binary), FileMode.Open);
            T save = (T)formatter.Deserialize(stream);
            stream.Close();
            return save;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data at {NameToURL(saveName, SaveType.Binary)}\nErrorMsg : {e.Message}");
            return default;
        }
    }
    public T LoadFromJson<T>(string saveName)
    {
        try
        {
            FileStream stream = File.Open(NameToURL(saveName, SaveType.Json), FileMode.Open);
            StreamReader reader = new StreamReader(stream);
            string txt = reader.ReadToEnd();
            T save = (T)JsonUtility.FromJson(txt, typeof(T));
            reader.Close();
            stream.Close();
            return save;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data at {NameToURL(saveName, SaveType.Json)}\nErrorMsg : {e.Message}");
            return default ;
        }
    }

    public bool Delete(string saveName,SaveType saveType)
    {
        if (!File.Exists(NameToURL(saveName, saveType)))
            return false;

        File.Delete(NameToURL(saveName, saveType));
        GetAllSaveFilesURL();
        return true;
    }

    void GetAllSaveFilesURL()
    {
        if (!Directory.Exists(SaveDirectory))
            return;

        saveFilesURL =  new List<string>(Directory.GetFiles(SaveDirectory));
    }

    static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();
        Vector3SerializationSurrogate v3Surrogate = new Vector3SerializationSurrogate();
        Vector2SerializationSurrogate v2Surrogate = new Vector2SerializationSurrogate();
        QuaternionSerializationSurrogate quaSurrogate = new QuaternionSerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
        selector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), v2Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }

    #region PlayerPrefs
    public void SaveToPlayerPrefs(string key, object data)
    {
        PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
    }
    public T LoadFromPlayerPrefs<T>(string key)
    {
        try
        {
            T obj = (T)JsonUtility.FromJson(key, typeof(T));
            return obj;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to load data from PlayerPrefs [{key}]\nErrorMsg : {e.Message}");
            return default;
        }
    }
    #endregion

    #region MenuItem
#if UNITY_EDITOR_WIN
    [MenuItem("Utility/Save/Delete All SaveFiles")]
    static void DeleteAllSaveFiles()
    {
        if (!Directory.Exists(SaveDirectory))
            return;
        try
        {
            foreach (var url in Directory.GetFiles(SaveDirectory))
                File.Delete(url);
            Debug.Log("Delete all save data successfully");
        }
        catch(System.Exception e)
        {
            Debug.LogError($"Failed to delete save data at {SaveDirectory}\n{e.Message}");
        }
    }
    [MenuItem("Utility/Save/Delete All PlayerPrefsKeys")]
    static void ClearPlayerPrefsKeys()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Delete all save keys successfully");
    }
    [MenuItem("Utility/Save/Show In Explorer")]
    static void ShowInExplorer()
    {
        EditorUtility.RevealInFinder(SaveDirectory);
    }
#endif
    #endregion
}

#region SerializationSurrogate
public class Vector3SerializationSurrogate : System.Runtime.Serialization.ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector3 v3 = (Vector3)obj;
        info.AddValue("x", v3.x);
        info.AddValue("y", v3.y);
        info.AddValue("z", v3.z);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector3 v3 = (Vector3)obj;
        v3.x = (float)info.GetValue("x", typeof(float));
        v3.y = (float)info.GetValue("y", typeof(float));
        v3.z = (float)info.GetValue("z", typeof(float));
        obj = v3;
        return obj;
    }
}

public class Vector2SerializationSurrogate : System.Runtime.Serialization.ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector2 v2 = (Vector2)obj;
        info.AddValue("x", v2.x);
        info.AddValue("y", v2.y);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector2 v2 = (Vector2)obj;
        v2.x = (float)info.GetValue("x", typeof(float));
        v2.y = (float)info.GetValue("y", typeof(float));
        obj = v2;
        return obj;
    }
}

public class QuaternionSerializationSurrogate : System.Runtime.Serialization.ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Quaternion quaternion = (Quaternion)obj;
        info.AddValue("x", quaternion.x);
        info.AddValue("y", quaternion.x);
        info.AddValue("z", quaternion.x);
        info.AddValue("w", quaternion.w);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Quaternion quaternion = (Quaternion)obj;
        quaternion.x = (float)info.GetValue("x", typeof(float));
        quaternion.y = (float)info.GetValue("y", typeof(float));
        quaternion.z = (float)info.GetValue("z", typeof(float));
        quaternion.w = (float)info.GetValue("w", typeof(float));
        obj = quaternion;
        return obj;
    }
}


#endregion
