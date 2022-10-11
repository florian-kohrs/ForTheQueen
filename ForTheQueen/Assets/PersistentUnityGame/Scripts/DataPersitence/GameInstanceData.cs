using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInstanceData
{

    public static GameInstanceData CurrentGameInstanceData => GamePersistence.GetCurrentGameInstanceData();

    public WorldData worldData = new WorldData();

    public GameTime timeData = new GameTime();

    public int startSeed;

}
