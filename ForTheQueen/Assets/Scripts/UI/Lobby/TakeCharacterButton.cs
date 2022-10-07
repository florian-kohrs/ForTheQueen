using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeCharacterButton : MonoBehaviourPun
{

    public int HeroIndex => heroDesigner.transform.GetSiblingIndex();

    public HeroDesigner heroDesigner;

    public GameObject activateOnTake;

    public Hero hero;

    private void Start()
    {
        activateOnTake.SetActive(false);
        hero.heroIndex = HeroIndex;
    }

    public void TakeControll()
    {
        int myId = PhotonNetworkExtension.LocalPlayerId;
        Broadcast.SafeRPC(photonView, nameof(PlayerTakesControllOverCharacter), RpcTarget.All, () => PlayerTakesControllOverCharacter(myId), myId);
    }

    [PunRPC]
    public void PlayerTakesControllOverCharacter(int playerIndex)
    {
        activateOnTake.SetActive(true);
        gameObject.SetActive(false);
        hero.ownerRoomId = playerIndex;
        hero.heroIndex = HeroIndex;
        Heroes.SetHero(hero, HeroIndex);
        heroDesigner.StartDesigner(hero);
    }

}
