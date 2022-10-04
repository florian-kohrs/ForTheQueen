using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonNetworkExtension
{

    public static bool IsMasterClient => PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected;

}
