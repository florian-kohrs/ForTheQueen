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

    public static void SafeRPC(string name, RpcTarget target, Action callIfNotConnected, params object[] parameters)
    {
        Broadcast.SafeRPC(Instance.photonView, name, target, callIfNotConnected, parameters);
    }

    [PunRPC]
    public void CreateWorld(int seed)
    {
        world.SpawnWorldWithSeed(seed);
    }

    //[PunRPC]
    //public void LoadWorld(byte[] savedFile)
    //{
    //    world.SpawnWorldWithSeed();
    //}

}
