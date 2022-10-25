using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class GameSaveData 
{

    public const string SAVE_NAME = "ForTheQueenSaves";

    public const string TEMP_NAME = "Temp";
    public const string DEVELOPMENT_GAME_NAME = "DEV";

    public static readonly string SAVE_DIRECTORY = FolderSystem.getGameDirectory(SAVE_NAME);
    public static readonly string SAVE_PATH = FolderSystem.getGameSavePath(SAVE_NAME);

    public Settings settings;

    public Dictionary<string, GameInstanceData> data = new Dictionary<string, GameInstanceData>();

    public static string currentGameDataName;

    public static bool HasGameWithName(string name)
    {
        return Instance.data.ContainsKey(name);
    }

    public static GameInstanceData CurrentGameData
    {
        get
        {
            if(string.IsNullOrEmpty(currentGameDataName))
            {
                Debug.LogWarning("No current game. Creating one. should only happen during developement");
                Instance.CreateNewGame(DEVELOPMENT_GAME_NAME);
            }
            return Instance.data[currentGameDataName];
        }
    }

    public static bool HasCurrentGame => Instance.data.ContainsKey(currentGameDataName);

    private static GameSaveData instance;

    public static Settings Settings => Instance.settings;

    static GameSaveData()
    {
        Debug.Log($"Game is saved here: {SAVE_PATH}");
    }

    public static GameSaveData Instance
    {
        get
        {
            if (instance == null)
            {
                if(File.Exists(SAVE_PATH))
                    instance = LoadSaveable<GameSaveData>(SAVE_PATH);
                else
                {
                    instance = new GameSaveData();
                    instance.Save();
                }
            }
            return instance;
        }
    }

    public void CreateNewGame(string gameName)
    {
        data[gameName] = new GameInstanceData();
        currentGameDataName = gameName;
    }

    public void AddGameInstanceData(GameInstanceData d)
    {
        data[TEMP_NAME] = d;
        currentGameDataName = TEMP_NAME;
    }

    public void Save()
    {
        FolderSystem.createPath(SAVE_DIRECTORY);
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(SAVE_PATH))
        {
            bf.Serialize(file, this);
        }
    }

    public GameInstanceData LoadLastSaveOfCurrentGame()
    {
        return LoadGame(currentGameDataName);
    }

    public GameInstanceData LoadGame(string name)
    {
        instance = LoadSaveable<GameSaveData>(SAVE_PATH);
        currentGameDataName = name;
        return Instance.data[currentGameDataName];
    }


    public static T LoadSaveable<T>(string path)
    {
        return WorkOnFileStreamOfSaveable(path, (f) => { f.Position = 0; return (T)new BinaryFormatter().Deserialize(f); });
    }

    public static T WorkOnFileStreamOfSaveable<T>(string path, System.Func<FileStream, T> f)
    {
        T result = default;
        //Debug.Log("loadthis." + path);
        if (File.Exists(path))
        {
            using FileStream file = File.OpenRead(path);
            result = f(file);
        }
        else
        {
            throw new DirectoryNotFoundException("Directory not found: " + path);
        }
        return result;
    }

}
