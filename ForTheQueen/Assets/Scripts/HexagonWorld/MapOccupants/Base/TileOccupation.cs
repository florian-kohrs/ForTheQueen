using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class TileOccupation<T> : ITileOccupation where T : TileOccupationScritableObject
{

    public TileOccupation() { }

    public TileOccupation(T occupationObject) 
    {
        this.occupationObject = occupationObject;
    }

    protected T occupationObject;

    public T OccupationObject => occupationObject;

    [System.NonSerialized]
    protected GameObject mapOccupationInstance;

    public MapTile mapTile;

    public MapTile MapTile
    {
        get
        {
            return mapTile;
        }
        set
        {
            mapTile = value;
        }
    }

    public abstract bool CanBeCrossed { get; }
    public abstract bool CanBeEntered { get; }

    public abstract void OnPlayerMouseExit();
    public abstract void OnPlayerEntered();
    public abstract void OnPlayerMouseHover();
    public abstract void OnPlayerReachedFieldAsTarget();
    public abstract void OnPlayerUncovered();

    public void SpawnOccupation(Transform parent)
    {
        mapOccupationInstance = occupationObject.Spawn(parent);
        mapOccupationInstance.transform.localPosition += MapTile.CenterPos;
    }
}
