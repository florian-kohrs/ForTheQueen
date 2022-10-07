using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainMenue : MenueInterfaceMask
{

    public MenueInterfaceMask newGameScreen;

    public MenueInterfaceMask loadGameScreen;

    public MenueInterfaceMask marketScreen;

    protected override void OnStart()
    {
        interfaceController.AddMask(this);
    }

    public void NewGame()
    {
        ChangeInterfaceFor(newGameScreen);
    }


    public void LoadGame()
    {
        ChangeInterfaceFor(loadGameScreen);
    }

    public void OpenMarket()
    {
        ChangeInterfaceFor(marketScreen);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
