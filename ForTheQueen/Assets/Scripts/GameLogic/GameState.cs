using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{

    protected int herosTurn;

    public static HexagonWorld world;


    private void Start()
    {
        StartPlayersTurn();
    }

    protected void NextHerosTurn()
    {
        herosTurn++;
        herosTurn %= Heros.NUMBER_PLAYERS;
        if(herosTurn == 0)
        {
            EndDay();
        }
    }

    public void EndCurrentHerosTurn()
    {
        NextHerosTurn();
        StartPlayersTurn();
    }

    protected void StartPlayersTurn()
    {
        Player.LocalPlayer.BeginPlayersHeroTurn(herosTurn);
    }

    protected void EndDay()
    {

    }


}
