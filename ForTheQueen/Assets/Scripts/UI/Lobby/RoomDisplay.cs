using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomDisplay : MonoBehaviour
{

    public NetworkLobbyJoiner joiner;

    public TMPro.TextMeshProUGUI roomName;

    public TMPro.TextMeshProUGUI playerInfo;

    public Button button;

    public void ShowRoom(RoomInfo r, NetworkLobbyJoiner joiner)
    {
        this.joiner = joiner;
        string roomName = string.Copy(r.Name);
        this.roomName.text = $"Room name:{Environment.NewLine}{roomName}";
        playerInfo.text = $"{r.PlayerCount} / {r.MaxPlayers} players";
        button.onClick.AddListener(delegate { joiner.JoinRoomByName(roomName); });
    }

}
