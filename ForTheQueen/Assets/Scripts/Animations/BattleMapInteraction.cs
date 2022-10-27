using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMapInteraction : BaseMapMovement<HeroCombat, BattleMapTile>
{

    protected override HeroCombat CurrentAgent => HeroCombat.currentHeroTurnInCombat;

    protected override HexagonGrid<BattleMapTile> Grid => map;

    public BattleMap map;

    public CombatState combatState;

    public BattleMapMovementAnimation anim;

    public MouseToHovoredBattleMapTile mouseSubscriber;

    protected int RangeModifier => DisplayActionImpact ? combatState.ActiveParticipant.CurrentAttackRange : 0;

    protected bool attackAfterMovement;

    protected int selectedActionIndex;

    protected bool HasReachedTargetOverride(HexagonPathfinder p, Vector2Int v1, Vector2Int v2)
    {
        if (map.DataFromIndex(v2).IsOccupied)
            return p.DistanceToTarget(v1, v2) <= Mathf.Max(1, RangeModifier);
        else
            return v1 == v2;
    }

    protected override Func<HexagonPathfinder, Vector2Int, Vector2Int, bool> ReachTargetOverride => HasReachedTargetOverride;

    public override void AnimateMovement(Vector2Int[] path)
    {
        anim.AnimateMovement(path.ToList(), CurrentAgent, OnMovementEnded);
    }

    protected void OnMovementEnded()
    {
        if(attackAfterMovement)
            CurrentAgent.ExecuteAction(selectedActionIndex, currentHoveredTile.Coordinates);
        OnEndMovement();
    }

    public override void RegisterMouseCallback()
    {
        mouseSubscriber.subscribers.AddSubscriber(this);
    }

    public override void UnregisterMouseCallback()
    {
        mouseSubscriber.subscribers.RemoveSubscriber(this);
    }

    protected override void OnClickOnValidField(Vector2Int v2)
    {
        IBattleParticipant p = currentHoveredTile.participant;
        if (p != null && !CurrentAgent.SelectedCombatAction.IsValidTarget(CurrentAgent, p))
        {
            Debug.Log("Selected target not valid for attack");
        }
        else
        {
            selectedActionIndex = CurrentAgent.currentSelectedCombatActionIndex;
            attackAfterMovement = CanAttack;
            base.OnClickOnValidField(v2);
        }
    }

    protected bool DisplayActionImpact => !Input.GetKey(KeyCode.LeftShift) && CurrentAgent.SelectedCombatAction != null;
    protected bool CanAttack => DisplayActionImpact && CurrentAgent.SelectedCombatAction.IsValidTarget(CurrentAgent,map.DataFromIndex(currentHoveredTile.Coordinates).participant);

    protected override bool IsCurrentHovoredTileInRange
    {
        get
        {
            if (CanAttack)
                return pathToCurrentHovoredTile.Count <= RestMovementInTurn + RangeModifier;
            else
                return pathToCurrentHovoredTile.Count <= RestMovementInTurn;
        }
    }

    protected override void OnBeginTileHover(Vector2Int coord)
    {
        map.RemovePreviousMarkers();
        if (!CanAttack)
            return;

        map.MarkActionOnMap(coord, CurrentAgent.SelectedCombatAction);
    }

}
