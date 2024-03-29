﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player LocalPlayer => PhotonNetwork.LocalPlayer.TagObject as Player;

    public PlayerMapMovement mapMovement;

    public MouseWorldEvents mouseTileHover;

    public static bool IsMyTurn => LocalPlayer.currentActiveHero.IsMine;

    protected Hero currentActiveHero;

    public static Hero CurrentActiveHero
    {
        get
        {
            if (LocalPlayer.currentActiveHero == null)
                LocalPlayer.currentActiveHero = Heroes.GetHeroWithActiveTurn();
            return LocalPlayer.currentActiveHero;
        }
    }

    public CallbackSet<IPlayerTurnStateListener> onPlayerTurnStatusChange = new CallbackSet<IPlayerTurnStateListener>();

    public static CallbackSet<IPlayerTurnStateListener> PlayerTurnStateCallbackSet => LocalPlayer.onPlayerTurnStatusChange;

    public void BeginPlayersHeroTurn(int index)
    {
        currentActiveHero = Heroes.GetHeroFromID(index);
        currentActiveHero.StartHerosTurn();
        ContinueHerosTurn();
    }

    public void ContinueHerosTurn()
    {
        currentActiveHero = Heroes.GetHeroWithActiveTurn();
        if (IsMyTurn)
        {
            GameManager.blockPlayerActiveAction.Remove(this);
            GameManager.blockPlayerMovement.Remove(this);
        }
        else
        {
            GameManager.blockPlayerActiveAction.Add(this);
            GameManager.blockPlayerMovement.Add(this);
        }
    }

    private void Awake()
    {
        PhotonNetwork.LocalPlayer.TagObject = this;
    }

    private void OnDestroy()
    {
        GameManager.blockPlayerActiveAction.Remove(this);
        GameManager.blockPlayerMovement.Remove(this);
    }

    public void EndTurn(Hero h)
    {
        GameManager.blockPlayerActiveAction.Add(this);
        GameManager.blockPlayerMovement.Add(this);
    }


}

