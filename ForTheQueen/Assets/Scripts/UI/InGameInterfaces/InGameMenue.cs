using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenue : InterfaceMask
{

    public GameObject settings;

    public void SaveGame()
    {
        if (!GameSaveData.HasCurrentGame)
            GameSaveData.Instance.CreateNewGame("TestSave");
        GameSaveData.Instance.Save();
    }

    public void LoadGame()
    {
        if (GameSaveData.HasCurrentGame)
            GameSaveData.Instance.LoadLastSaveOfCurrentGame();
        else
            GameSaveData.Instance.LoadGame("TestSave");
        PhotonNetwork.LoadLevel(StartGame.GAME_SCENE_NAME);
    }

    public void Continue()
    {
        interfaceController.RemoveMask(this);
    }

    public void OpenSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }


    public void SafeAndExit()
    {
        SaveGame();
        Application.Quit();
    }

    public override CursorLockMode CursorMode => CursorLockMode.None;

    public override bool BlockCameraMovement => true;

    public override bool BlockPlayerMovement => true;

    public override bool BlockPlayerActiveAction => true;

    public override bool BlockPlayerPassiveAction => true;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsOpen)
                interfaceController.RemoveMask(this);
            else
                interfaceController.AddMask(this);
        }
    }

}
