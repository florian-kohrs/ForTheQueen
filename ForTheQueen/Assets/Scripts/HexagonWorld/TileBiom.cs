using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileBiom : SaveableScriptableObject
{

    public float maxMovementOnBiom = 1;

    public Color color;

    public List<TownScriptableObject> townsInBiom;

    public List<TileBlockingOccupationObject> blockingOccupations;

    public List<SingleEnemyOccupationScripableObject> singleEnemiesInBiom;

    //Boss?

    [Range(0,100), Tooltip("How many percent of tiles should be occupied")]
    public int blockingOccupationDensity = 20;

    [Range(0,100)]
    public int enemyDensity = 12;

    [Range(0,10)]
    public int fightAssistRange = 2;

}
