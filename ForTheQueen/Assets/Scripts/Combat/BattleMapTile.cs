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

    public bool CanBeEntered => participant == null;


}
