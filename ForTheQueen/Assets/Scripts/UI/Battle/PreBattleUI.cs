using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreBattleUI : AdaptableInterfaceMask<BattleParticipants>
{

    public Transform rightPartyParent;
    public Transform leftPartyParent;

    public override bool BlockCameraMovement => true;

    public override bool BlockPlayerMovement => true;

    public override bool BlockPlayerActiveAction => true;

    public override bool BlockPlayerPassiveAction => true;

    protected override void AdaptUITo(BattleParticipants participants, Vector3 pos)
    {
        //TODO: Let armor and equipment from enemies grow legs and run away so they cant be looted.
        //sometimes they dont and will be looted

        Debug.Log($"{participants.onEnemiesSide.Count} emies against {participants.onPlayersSide.Count} heroes");

        foreach (var enemy in participants.onEnemiesSide)
        {
            enemy.DisplayInPreFight(rightPartyParent);
        }

        foreach (var hero in participants.onPlayersSide)
        {
            hero.DisplayInPreFight(leftPartyParent);
        }


    }
}
