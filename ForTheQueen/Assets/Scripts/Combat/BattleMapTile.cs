using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapTile : BaseMapTile
{

    public BattleMapTile() { }

    public BattleMapTile(Vector2Int coordinates, BaseHexagonGrid grid) : base(coordinates, grid)
    {
    }

    public IBattleParticipant participant;

    public bool IsOccupied => participant != null;

    public override bool CanBeEntered(bool allowWater)
    {
        return participant == null;
    }

    public override bool IsValidMovementTarget(bool allowWater)
    {
        return true;
    }
}
