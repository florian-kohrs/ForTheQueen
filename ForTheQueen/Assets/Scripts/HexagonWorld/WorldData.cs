using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WorldData
{

    public MapTile[,] world;

    public List<Continent> contintents = new List<Continent>();

    public bool IsWorldEmpty => contintents.Count == 0;

}
