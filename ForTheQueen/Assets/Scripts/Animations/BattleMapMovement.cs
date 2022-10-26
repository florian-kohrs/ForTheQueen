using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMapMovement : BaseMapMovement<HeroCombat, BattleMapTile>
{

    protected override HeroCombat CurrentAgent => HeroCombat.currentHeroTurnInCombat;

    protected override HexagonGrid<BattleMapTile> Grid => map;

    public BattleMap map;

    public BattleMapMovementAnimation anim;

    public MouseToHovoredBattleMapTile mouseSubscriber;

    protected bool HasReachedTargetOverride(HexagonPathfinder p, Vector2Int v1, Vector2Int v2)
    {
        if (map.DataFromIndex(v2).IsOccupied)
            return p.DistanceToTarget(v1, v2) <= 1;
        else
            return v1 == v2;
    }

    protected override Func<HexagonPathfinder, Vector2Int, Vector2Int, bool> ReachTargetOverride => HasReachedTargetOverride;

    public override void AnimateMovement(Vector2Int[] path)
    {
        anim.AnimateMovement(path.ToList(), CurrentAgent, OnEndMovement);
    }

    public override void RegisterMouseCallback()
    {
        mouseSubscriber.subscribers.AddSubscriber(this);
    }

    public override void UnregisterMouseCallback()
    {
        mouseSubscriber.subscribers.RemoveSubscriber(this);
    }

}
