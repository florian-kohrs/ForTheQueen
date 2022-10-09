using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TileInteractableOccupation<T> : TileOccupation<T> 
    where T : TileOccupationInteractableScritableObject
{

    public TileInteractableOccupation() { }

    public TileInteractableOccupation(T t) : base(t)
    { }

    public override bool CanBeCrossed => true;

    public override bool CanBeEntered => true;


}
