using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInteractions : MonoBehaviourPun
{

    public GameState gameState;

    public void EndHeroesTurn()
    {
        Broadcast.SafeRPC(photonView, nameof(EndHeroesTurnRPC), RpcTarget.All, EndHeroesTurnRPC);
    }
    
    [PunRPC]
    public void EndHeroesTurnRPC()
    {
        gameState.EndCurrentHerosTurn();
    }

}
