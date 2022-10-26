using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBattleParticipant : InventoryCreatureCombat
{

    public EnemyBattleInfoUI enemyUI;

    public List<CombatAction> actions;

    public List<CombatAction> Actions
    {
        set
        {
            actions = value;
            if(inventory != null)
                actions.AddRange(inventory.AvailableWeaponActions);
        }
        get => actions;
    }

    public string npcName;

    public bool isOnPlayersSide;

    public override string Name => npcName;

    public override bool OnPlayersSide => isOnPlayersSide;

    private void Awake()
    {
        UIReferences.Add(enemyUI);
    }

    protected override IHealthDisplayer HealthDisplayer => enemyUI;

    protected override List<CombatAction> AllCombatActions => Actions;

    public override int MovementInTurn => 3;

    public override void OnStartTurn()
    {
        Debug.Log("Enemy starts turn. attack then end turn");
        Attack();
    }

    protected override void AfterActionExecuted()
    {
        CombatState.EndTurn();
    }

    public override void OnTurnEnded()
    {
        Debug.Log("NPC ended turn");
    }

    protected void Attack()
    {
        int actionIndex = GameInstanceData.Rand.Next(0,Actions.Count);
        List<Vector2Int> heroPositions = CombatState.FieldsWithHeroes;
        Vector2Int targetField = heroPositions[GameInstanceData.Rand.Next(0, heroPositions.Count)];
        CombatState.NPCAttack(actionIndex, targetField);
    }



    protected override void OnDeath()
    {
        CombatState.ParticipantDied(this);
    }
}
