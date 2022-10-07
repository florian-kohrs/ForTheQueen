using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviourPun
{

    public const string GAME_SCENE_NAME = "GameScene";

    public void BeginAdventure()
    {
        if(Heroes.AllHeroesInitialized)
        {
            Broadcast.SafeRPC(photonView, nameof(BeginAdventureRPC), RpcTarget.All, () => BeginAdventureRPC());
        }
        else
        {
            Debug.Log("Cant start game before all players are ready.");
        }
    }

    [PunRPC]
    public void BeginAdventureRPC()
    {
        PhotonNetwork.LoadLevel(GAME_SCENE_NAME);
    }

}
