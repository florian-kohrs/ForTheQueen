using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyJoinerInterface : MenueInterfaceMask
{

    public NetworkLobbyJoiner joiner;

    protected override void OnOpen()
    {
        joiner.enabled = true;
    }

    protected override void OnClose()
    {
        joiner.enabled = false;
    }

}
