using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlockingOccupationInstance : TileOccupation<TileBlockingOccupationObject>
{
    public TileBlockingOccupationInstance() { }

    public TileBlockingOccupationInstance(TileBlockingOccupationObject occupationObject) : base(occupationObject) { }

    public override bool CanBeCrossed => false;

    public override bool CanBeEntered => false;

    public override void OnPlayerEntered(Hero p)
    {
    }

    public override void OnPlayerMouseExit(Hero p)
    {
    }

    public override void OnPlayerMouseHover(Hero p)
    {
    }

    public override void OnPlayerReachedFieldAsTarget(Hero p)
    {
    }

    public override void OnPlayerUncovered(Hero p)
    {
    }
}
