using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownScriptableObject : TileOccupationInteractableScritableObject
{

    public List<ItemContainer> guaranteedItemsToSell;

    public Sprite townSprite;

    public bool isStartTown;

}
