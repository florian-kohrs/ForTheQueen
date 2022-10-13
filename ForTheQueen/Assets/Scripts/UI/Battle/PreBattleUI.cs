using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreBattleUI : AdaptableInterfaceMask<MapMovementBattleInteruption>
{

    public Transform rightPartyParent;
    public Transform leftPartyParent;

    public Button sneakBtn;

    protected MapMovementBattleInteruption battleInteruption;

    public void BtnFight()
    {
        PunBroadcastCommunication.SafeRPC(nameof(PunBroadcastCommunication.StartFight), RpcTarget.All, Fight);
    }

    public void BtnSneak()
    {
        int seed = Random.Range(0, 99999999);
        PunBroadcastCommunication.SafeRPC(nameof(PunBroadcastCommunication.SneakFromFight), RpcTarget.All, ()=>Sneak(seed),seed);
    }

    public void BtnRetreat()
    {
        PunBroadcastCommunication.SafeRPC(nameof(PunBroadcastCommunication.RetreatFromFight), RpcTarget.All, Retreat);
    }

    public void Fight()
    {
        RemoveMask();
    }

    public void Sneak(int seed)
    {
        battleInteruption.mapMovement.ContinuePath();
    }

    public void Retreat()
    {
        RemoveMask();
        battleInteruption.mapMovement.ContinuePath();
    }

    protected void SetInteractableOfButtons()
    {
        foreach (var item in GetComponentsInChildren<Button>())
        {
            item.interactable = Player.IsMyTurn;
        }  
    }

    public override bool BlockCameraMovement => true;

    public override bool BlockPlayerMovement => true;

    public override bool BlockPlayerActiveAction => true;

    public override bool BlockPlayerPassiveAction => true;

    protected override void AdaptUITo(MapMovementBattleInteruption battleInteruption, Vector3 pos)
    {
        //TODO: Let armor and equipment from enemies grow legs and run away so they cant be looted.
        //sometimes they dont and will be looted
        this.battleInteruption = battleInteruption;
        BattleParticipants participants = battleInteruption.participants;
        Debug.Log($"{participants.onEnemiesSide.Count} emies against {participants.onPlayersSide.Count} heroes");

        foreach (var enemy in participants.onEnemiesSide)
        {
            enemy.DisplayInPreFight(rightPartyParent);
        }

        foreach (var hero in participants.onPlayersSide)
        {
            hero.DisplayInPreFight(leftPartyParent);
        }

        ///only activate sneak button if the path didnt end on enemy
        sneakBtn.gameObject.SetActive(!battleInteruption.mapMovement.IsPathDone);

        SetInteractableOfButtons();
    }

}
