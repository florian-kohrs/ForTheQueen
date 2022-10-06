using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenue : InterfaceMask
{

    public GameObject settings;

    public void SaveGame()
    {
        SaveLoad.Save();
    }

    public void LoadGame()
    {
        SaveLoad.Load();
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

    protected override void OnOpen()
    {
        GameManager.FreezeCamera();
    }

    protected override void OnClose()
    {
        GameManager.UnfreezeCamera();
    }

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
