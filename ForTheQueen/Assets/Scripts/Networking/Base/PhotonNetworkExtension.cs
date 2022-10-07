using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonNetworkExtension
{

    public static bool IsMasterClient => PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected;

    public static int LocalPlayerId
    {
        get
        {
            if (PhotonNetwork.IsConnected)
                return PhotonNetwork.LocalPlayer.ActorNumber;
            else
                return 0;
        }
    }

}
