using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    public static T[] FromObjectArray<T>(object[] os) => os.OfType<T>().ToArray();


    public static object[] ToObjectArray<T>(IEnumerable<T> ts) => ts.ToArray().OfType<object>().ToArray();

}
