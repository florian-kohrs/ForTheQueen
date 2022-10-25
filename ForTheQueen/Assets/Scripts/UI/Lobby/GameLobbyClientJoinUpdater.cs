using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameLobbyClientJoinUpdater : MonoBehaviourPunCallbacks
{

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            int seed = GameInstanceData.CurrentGameInstanceData.StartSeed;
            Broadcast.SafeRPC(photonView, nameof(SetSeed), RpcTarget.Others, () => SetSeed(seed),seed);
        }
    }

    public void SetSeed(int seed)
    {
        GameInstanceData.CurrentGameInstanceData.SetSeed(seed);
    }

}
