using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBattleParticipant : InventoryCreatureCombat
{
    
    public List<CombatAction> actions;

    public override void StartTurn()
    {
        Debug.Log("Enemy starts turn. end turn immediatly");
        CombatState.EndTurn();
    }

}
