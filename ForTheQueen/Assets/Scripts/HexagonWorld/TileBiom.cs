using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileBiom : SaveableScriptableObject
{

    public float maxMovementOnBiom = 1;

    public Color color;

    public List<TownScriptableObject> townsInBiom;

    public List<TileBlockingOccupation> blockingOccupations;

    public float blockingOccupationDensity = 1;

}
