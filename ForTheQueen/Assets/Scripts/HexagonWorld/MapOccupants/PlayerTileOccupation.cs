using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileOccupation : TileOccupationCreature
{

    public override bool CanBeCrossed => true;

    public override void OnPlayerEntered()
    {
        ///Maybe move player to the side a bit
    }
}
