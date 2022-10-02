using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileOccupationCreature : MonoBehaviour, ITileOccupation
{

    public abstract bool CanBeCrossed { get; }

    public bool CanBeEntered => true;

    public abstract void OnPlayerMouseHover();
}
