using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town : TileInteractableOccupation<TownScriptableObject, Town, TownUI>
{

    public Town() { }

    public Town(TownScriptableObject town) : base(town)
    { }

    public Inventory itemsToSell;

    public string TownName => occupationObject.occupationName;

    public override void OnPlayerEntered()
    {
    }

    public override void OnPlayerReachedFieldAsTarget()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerUncovered()
    {
    }

}
