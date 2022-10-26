using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementAgent
{
    
    int MovementRemaining { get; set; }

    Vector2Int CurrentTile { get; set; }

    bool CanEnterWater { get; }

    Transform RuntimeHeroObject { get; } 

}
