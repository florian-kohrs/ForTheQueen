using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SingleEnemyOccupation : BaseEnemyOccupation<SingleEnemyOccupationScripableObject>
{

    protected CreatureStats statsClone;

    public SingleEnemyOccupation() { }

    public SingleEnemyOccupation(SingleEnemyOccupationScripableObject e) : base(e) { }

    public override bool HasSupportRange => OccupationObject.hasSupportRange;

    public override bool HelpsInFight => true;

    public override void DisplayInPreFight(Transform parent)
    {
    }

    protected override void ApplyParticipantStats(NPCBattleParticipant p)
    {
        statsClone = (CreatureStats)OccupationObject.stats.Clone();
        statsClone.currentHealth = statsClone.MaxHealth;
        p.inventory = OccupationObject.items;
        p.Actions = new List<CombatAction>(OccupationObject.creatureActions);
        p.stats = statsClone;
        p.npcName = OccupationObject.occupationName;
        p.isOnPlayersSide = false;
    }
}
