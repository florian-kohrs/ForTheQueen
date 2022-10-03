using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileOccupationBehaviour : SaveableMonoBehaviour, ITileOccupation
{

    public MapTile MapTile { get; set; }
    public abstract bool CanBeCrossed { get; }
    public abstract bool CanBeEntered { get; }

    public abstract void OnPlayerMouseExit();
    public abstract void OnPlayerEntered();
    public abstract void OnPlayerMouseHover();
    public abstract void OnPlayerReachedFieldAsTarget();
    public abstract void OnPlayerUncovered();

    public void SpawnOccupation(Transform parent)
    {
        throw new System.NotImplementedException();
    }
}

