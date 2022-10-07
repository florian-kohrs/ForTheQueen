using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkLobbyCreator : MonoBehaviourPunCallbacks
{

    public const int MAX_PLAYERS = 3;

    public const string LOBBY_SCENE_NAME = "CharacterCreation";


    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    private string gameVersion = "1";

    protected bool createdRoom = false;

    protected string gameName;

    public static bool IsConnected { get;protected set; }


    public void BeginnOnlineGame(string gameName)
    {
        this.gameName = gameName;
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            CreateOnlineRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            IsConnected = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            if(!IsConnected)
            {
                CreateOfflineLobby();
            }
        }
    }

    public void BeginnOfflineGame(string gameName)
    {
        CreateOfflineLobby();
    }

    public override void OnConnectedToMaster()
    {
        CreateOnlineRoom();
    }

    protected void CreateOnlineRoom()
    {
        if (createdRoom)
        {
            Debug.LogWarning("Room was already created by this client");
        }
        else
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            createdRoom = true;
            PhotonNetwork.CreateRoom(gameName, new RoomOptions { MaxPlayers = MAX_PLAYERS });
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateOfflineLobby();
    }

    protected void CreateOfflineLobby()
    {
        IsConnected = false;
        Debug.Log("Created offline room");
        SceneManager.LoadScene(LOBBY_SCENE_NAME);
    }

    public override void OnCreatedRoom()
    {
        IsConnected = true;
        Debug.Log("Photon Pun created room");
        PhotonNetwork.LoadLevel(LOBBY_SCENE_NAME);
    }

}
