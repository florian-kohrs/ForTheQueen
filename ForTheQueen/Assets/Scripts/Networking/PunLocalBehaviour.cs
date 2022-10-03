using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunLocalBehaviour : MonoBehaviourPun
{

    protected void Start()
    {
        if(PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            OnNotMine();
        }
        else
        {
            OnStart();
        }
    }

    protected virtual void OnStart() { }

    protected virtual void OnNotMine() { Destroy(this); }

}
