using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGameMenue : MenueInterfaceMask
{

    public TMP_InputField gameName;

    public Button startGame;

    public NetworkLobbyCreator lobbyCreator;

    protected override void OnStart()
    {
        SetEnabledOfButton(startGame, false);
        gameName.onValueChanged.AddListener(EvaluateGameName);
    }

    protected void EvaluateGameName(string name)
    {
        bool isEmpty = string.IsNullOrEmpty(name);
        if(isEmpty)
        {
            SetEnabledOfButton(startGame, false);
            return;
        }

        if(GameSaveData.HasGameWithName(name))
        {
            Debug.LogWarning("Game with name exists already");
        }

        SetEnabledOfButton(startGame, true);
    }


    public void StartGame()
    {
        lobbyCreator.enabled = true;
        lobbyCreator.BeginnOnlineGame(gameName.text);
    }

}
