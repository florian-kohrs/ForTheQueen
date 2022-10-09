using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    private int activeHeroId;

    public static HexagonWorld world;

    public CallbackSet<IPlayerTurnStateListener> turnListener;

    private void Start()
    {
        Hero currentActive = Heroes.GetCurrentActiveHero();
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
        Heroes.GetCurrentActiveHero().EndHerosTurn();
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

    }

}
