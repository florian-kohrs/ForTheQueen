using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileOccupationCreature : TileOccupationBehaviour
{

    public override bool CanBeEntered => true;

    public override void OnPlayerMouseExit() { }

    public override void OnPlayerMouseHover() { }

    public override void OnPlayerReachedFieldAsTarget() { }

    public override void OnPlayerUncovered() { }

}
