using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainMenue : MenueInterfaceMask
{

    public NewGameMenue newGameScreen;

    public MenueInterfaceMask loadGameScreen;

    public LobbyJoinerInterface joinGameScreen;

    public MenueInterfaceMask marketScreen;

    protected override void OnStart()
    {
        interfaceController.AddMask(this);
    }

    public void NewGame()
    {
        OpenAdditiveInterface(newGameScreen);
    }

    public void JoinGame()
    {
        OpenAdditiveInterface(joinGameScreen);
    }

    public void LoadGame()
    {
        OpenAdditiveInterface(loadGameScreen);
    }

    public void OpenMarket()
    {
        OpenAdditiveInterface(marketScreen);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
