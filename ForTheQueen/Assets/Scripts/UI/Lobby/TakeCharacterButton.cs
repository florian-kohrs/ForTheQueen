using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCharacterButton : MonoBehaviourPun
{

    public int heroIndex;

    public HeroDesigner heroDesigner;

    public Hero hero;

    private void Start()
    {
        hero.heroIndex = heroIndex;
    }

    public void TakeControll()
    {
        int myId = PhotonNetworkExtension.LocalPlayerId;
        Broadcast.SafeRPC(photonView, nameof(PlayerTakesControllOverCharacter), RpcTarget.All, () => PlayerTakesControllOverCharacter(myId), myId);
    }

    [PunRPC]
    public void PlayerTakesControllOverCharacter(int playerIndex)
    {
        gameObject.SetActive(false);
        hero.ownerRoomId = playerIndex;
        Heroes.SetHero(hero, heroIndex);
        heroDesigner.StartDesigner(hero);
    }

}
