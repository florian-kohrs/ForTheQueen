using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Broadcast
{
    
    public static void SafeRPC(PhotonView view, string name, RpcTarget target, Action callIfNotConnected, params object[] parameters)
    {
        if(PhotonNetwork.IsConnected)
        {
            view.RPC(name, target, parameters);
        }
        else if(target != RpcTarget.Others && target != RpcTarget.OthersBuffered)
        {
            callIfNotConnected?.Invoke();
        }
    }

    public static void SafeRPCToOthers(PhotonView view, string name, params object[] parameters)
    {
        SafeRPC(view, name, RpcTarget.Others, null, parameters);
    }

    public static void SafeRPCToOthersBuffered(PhotonView view, string name, params object[] parameters)
    {
        SafeRPC(view, name, RpcTarget.OthersBuffered, null, parameters);
    }

}
