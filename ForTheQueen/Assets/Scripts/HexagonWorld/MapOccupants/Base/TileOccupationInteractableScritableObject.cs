using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileOccupationInteractableScritableObject : TileOccupationScritableObject 
{

    public string occupationName;

    [TextArea]
    public string description;

    public Color hoverBackgroundSpriteColor = Color.white;

}
