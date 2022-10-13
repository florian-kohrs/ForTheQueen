using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameInstanceData
{
    //TODO: Broadcast this!
    public void ShuffleGameInstanceSeed()
    {
        gameInstanceSeed = Random.Range(-99999999, 99999999);
        rand = null;
    }

    protected int gameInstanceSeed;

    [System.NonSerialized]
    protected System.Random rand;

    //TODO: Shuffle seed in lobby!

    public static System.Random Rand
    {
        get
        {
            if(CurrentGameInstanceData.rand == null)
                CurrentGameInstanceData.rand = new System.Random(GameInstanceSeed);
            return new System.Random(CurrentGameInstanceData.rand.Next());
        }
    }

    protected static int GameInstanceSeed => CurrentGameInstanceData.gameInstanceSeed;

    public static GameInstanceData CurrentGameInstanceData => GamePersistence.GetCurrentGameInstanceData();

    public WorldData worldData = new WorldData();

    public GameTime timeData = new GameTime();

    public int startSeed;

}
