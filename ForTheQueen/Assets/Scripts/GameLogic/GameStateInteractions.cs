using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInteractions : MonoBehaviourPun
{

    public GameState gameState;

    public void EndHerosTurn()
    {
        Broadcast.SafeRPC(photonView, nameof(gameState.EndCurrentHerosTurn), RpcTarget.All, ()=>gameState.EndCurrentHerosTurn());
    }

}
