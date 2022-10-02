using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Continent 
{


    public string ContinentName;

    public Vector2Int startCoord;

    public Vector2Int size;

    /// <summary>
    /// determines how much the noise value at position influences choice if tile is water or land
    /// </summary>
    protected AnimationCurve distanceNoiseWeighting;

    protected int[,] continentFactionAssignment;

    

    public Continent(Vector2Int startCoord, Vector2Int size, AnimationCurve distanceNoiseWeighting)
    {
        this.startCoord = startCoord;
        this.size = size;
        this.distanceNoiseWeighting = distanceNoiseWeighting;
        continentFactionAssignment = new int[size.x, size.y];
        BuildContinent();
    }


    public void WriteContinentFactionTilesIntoWorld(MapTile[,] world)
    {
        for (int x = 0; x < size.x; x++)
        {
            int currentX = startCoord.x + x;
            for (int y = 0; y < size.y; y++)
            {
                int currentFaction = continentFactionAssignment[x, y];
                if (currentFaction == 0)
                    continue;

                int currentY = startCoord.y + y;
                world[currentX, currentY] = new MapTile(new Vector2Int(currentX, currentY), currentFaction);
            }
        }
    }

    protected void BuildContinent()
    {
        DjikstraFactionAssignment.AssignFactionForContinent(continentFactionAssignment);
        MakeBorderMoreNatural();
    }


    protected void MakeBorderMoreNatural()
    {

    }


    protected bool IsTileLand(Vector2Int pos, Vector2Int size)
    {
        float distance = DistanceToBorder(pos, size);
        float influence = distanceNoiseWeighting.Evaluate(distance);
        if (influence <= 0)
            return true;

        //float noise = NoiseAt(pos) * influence
        //return noise;
        return true;
    }

    protected float DistanceToBorder(Vector2Int pos, Vector2Int size)
    {
        return 0;
    }

}
