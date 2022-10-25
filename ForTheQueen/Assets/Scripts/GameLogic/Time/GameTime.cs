using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTime
{

    protected const int ROUNDS_PER_DAY = 8;

    protected const int ROUNDS_PER_Night = 4;

    protected const int RESPAWN_MAP_OCCUPATION_FREQUENCY = 4;
    
    protected int playerRoundsDone = 0;

    public int PlayerRoundsDone => playerRoundsDone;

    public void EnterNextRound()
    {
        GameManager.FreezeAllActiveActions(this);
        playerRoundsDone++;
        if(playerRoundsDone % RESPAWN_MAP_OCCUPATION_FREQUENCY == 0)
        {
            RespawnTempOccupations();
        }
        GameManager.UnfreezeAllActiveActions(this);
    }

    public void StartGame()
    {
        CreateTempOccupations();
    }

    protected void RespawnTempOccupations()
    {
        RemoveTempOccupations();
        CreateTempOccupations();
    }

    protected void RemoveTempOccupations()
    {
        //foreach (var item in Continent)
        //{

        //}
    }

    protected void CreateTempOccupations()
    {
        System.Random rand = new System.Random(GameInstanceData.CurrentGameInstanceData.StartSeed * playerRoundsDone);
        GameInstanceData.CurrentGameInstanceData.worldData.contintents.ForEach(c =>
        c.RespawnEnemies(rand));
    }


}
