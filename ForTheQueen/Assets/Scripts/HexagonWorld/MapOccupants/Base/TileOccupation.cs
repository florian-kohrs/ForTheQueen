using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class TileOccupation<T> : ITileOccupation where T : TileOccupationScritableObject
{

    public TileOccupation() { }

    public TileOccupation(T occupationObject) 
    {
        OccupationObject = occupationObject;
    }

    protected AssetPolyRef<T> savedScriptableObject;

    public T OccupationObject
    {
        get
        {
            return savedScriptableObject.RuntimeRef;
        }
        set
        {
            savedScriptableObject = new AssetPolyRef<T>() { RuntimeRef = value };
        }
    }

    [NonSerialized]
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

    public abstract void OnPlayerMouseExit(Hero p);
    public abstract void OnPlayerEntered(Hero p);
    public abstract void OnPlayerMouseHover(Hero p);
    public abstract void OnPlayerReachedFieldAsTarget(Hero p);
    public abstract void OnPlayerUncovered(Hero p);

    public virtual void SpawnOccupation(Transform parent)
    {
        mapOccupationInstance = OccupationObject.Spawn(parent);
        mapOccupationInstance.transform.localPosition += MapTile.CenterPos;
    }

    public virtual void OnPlayerLeftFieldAfterStationary(Hero p) { }

}
