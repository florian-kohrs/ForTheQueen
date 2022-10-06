using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player LocalPlayer => PhotonNetwork.LocalPlayer.TagObject as Player;

    public PlayerMapMovement mapMovement;

    public MouseToHoveredMapTile mouseTileHover;

    public static bool IsMyTurn => LocalPlayer.isMyTurn;

    protected bool isMyTurn;

    protected Hero currentActiveHero;

    public static Hero CurrentActiveHero => LocalPlayer.currentActiveHero;

    //public CallbackSet<IOnMyPlayerTurnStatusChangesCallbacks> onPlayerTurnStatusChange;

    [PunRPC]
    public void BeginPlayersHeroTurn(int heroId)
    {
        currentActiveHero = Heros.GetHeroFromID(heroId);
        currentActiveHero.StartHerosTurn();
        isMyTurn = currentActiveHero.IsMine;
        if (isMyTurn)
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


    public void EndTurn(Hero h)
    {
        GameManager.blockPlayerActiveAction.Add(this);
        GameManager.blockPlayerMovement.Add(this);
    }


}

