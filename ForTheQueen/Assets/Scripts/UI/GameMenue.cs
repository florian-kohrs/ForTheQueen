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
        InterfaceController.RemoveMask(this);
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsOpen)
                interfaceController.RemoveMask_(this);
            else
                interfaceController.AddMask_(this);
        }
    }

}
