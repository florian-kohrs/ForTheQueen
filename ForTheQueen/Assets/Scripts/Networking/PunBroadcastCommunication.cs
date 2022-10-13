using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunBroadcastCommunication : MonoBehaviourPun
{

    private void Awake()
    {
        Instance = this;
    }

    public static PunBroadcastCommunication Instance { get; private set; }

    public HexagonWorld world;

    public GameState gameState;

    public static void SafeRPC(string name, RpcTarget target, Action callIfNotConnected, params object[] parameters)
    {
        Broadcast.SafeRPC(Instance.photonView, name, target, callIfNotConnected, parameters);
    }

    [PunRPC]
    public void StartFight()
    {
        InterfaceController.GetInterfaceMask<PreBattleUI>().Fight();
    }

    [PunRPC]
    public void SneakFromFight(int seed)
    {
        InterfaceController.GetInterfaceMask<PreBattleUI>().Sneak(seed);
    }

    [PunRPC]
    public void RetreatFromFight()
    {
        InterfaceController.GetInterfaceMask<PreBattleUI>().Retreat();
    }

    [PunRPC]
    public void EndCurrentPlayersTurn()
    {
        gameState.EndCurrentHerosTurn();
    }

}
