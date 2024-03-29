using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    private int activeHeroId;

    public CallbackSet<IPlayerTurnStateListener> turnListener;

    public HexagonWorld world;

    private void Awake()
    {
        HexagonWorld.instance.onWorldCreated.Add(OnWorldCreated);
    }

    protected void OnWorldCreated()
    {
        Hero currentActive = Heroes.GetHeroWithActiveTurn();
        if(currentActive == null)
        {
            activeHeroId = 0;
            StartHerosTurn();
        }
        else
        {
            activeHeroId = currentActive.heroIndex;
            ContinueHeroesTurn();
        }
    }

    protected void ProgressInitiativeTimeline()
    {
        activeHeroId++;
        activeHeroId %= Heroes.NUMBER_HEROES;
        if(activeHeroId == 0)
        {
            EndCurrentRound();
        }
    }

    public void EndCurrentHerosTurn()
    {
        Heroes.GetHeroWithActiveTurn().EndHerosTurn();
        ProgressInitiativeTimeline();
        StartHerosTurn();
    }

    protected void StartHerosTurn()
    {
        Player.LocalPlayer.BeginPlayersHeroTurn(activeHeroId);
    }

    protected void ContinueHeroesTurn()
    {
        Player.LocalPlayer.ContinueHerosTurn();
    }

    protected void EndCurrentRound()
    {
        GameInstanceData.CurrentGameInstanceData.timeData.EnterNextRound();
    }

}
