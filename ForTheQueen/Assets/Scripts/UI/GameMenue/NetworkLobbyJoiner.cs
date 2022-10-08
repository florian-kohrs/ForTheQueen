using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLobbyJoiner : MonoBehaviourPunCallbacks
{

    public TMPro.TMP_InputField inputField;

    public GameObject roomDisplayPrefab;

    public Transform roomDisplayParent;

    protected List<GameObject> currentRoomDisplays = new List<GameObject>();

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = NetworkLobbyCreator.GAME_VERSION;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
            OnConnectedToMaster();
        else
            PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        ScanOpenRooms();
    }

    public void ScanOpenRooms()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearCurrentRoomDisplay();

        foreach (var item in roomList)
        {
            if(!ShowRoom(item))
                continue;

            DisplayRoom(item);
        }
    }

    protected bool ShowRoom(RoomInfo r) => r.IsVisible && !r.RemovedFromList && r.PlayerCount < r.MaxPlayers;

    protected void DisplayRoom(RoomInfo r)
    {
        GameObject display = Instantiate(roomDisplayPrefab, roomDisplayParent);
        currentRoomDisplays.Add(display);
        display.GetComponent<RoomDisplay>().ShowRoom(r, this);
    }

    protected void ClearCurrentRoomDisplay()
    {
        foreach (GameObject room in currentRoomDisplays)
            Destroy(room);
    }

    public void JoinEnteredRoom()
    {
        JoinRoomByName(inputField.text);
    }

    public void JoinRoomByName(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed joining room. message: {message}");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"Joined Lobby");
    }

}
