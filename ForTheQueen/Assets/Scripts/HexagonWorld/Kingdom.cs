using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kingdom
{

    protected const int MIN_DISTANCE_BETWEEN_TOWNS = 8;


    public HashSet<MapTile> mapFieldsOfKingdom;

    public HexagonWorld world;

    public TileBiom kingdomBiom;

    public System.Random rand;

    public void ApplyOccupationToKingdom()
    {
        SpawnTowns();

        foreach (var tile in mapFieldsOfKingdom)
        {
            if
        }
    }

    public void SpawnTowns()
    {
        HashSet<MapTile> townFieldOfKingdom = new HashSet<MapTile>(mapFieldsOfKingdom);

        foreach (var townObject in kingdomBiom.townsInBiom)
        {
            MapTile t = Rand.PickOne(townFieldOfKingdom, rand);
            Town town = new Town(townObject);
            t.AddTileOccupation(town);
            foreach (var item in world.MapTilesFromIndices(HexagonPathfinder.GetNeighboursInDistance(t.Coordinates, MIN_DISTANCE_BETWEEN_TOWNS)))
            {
                townFieldOfKingdom.Remove(item);
            }
            if (town.OccupationObject.isStartTown)
            {
                Heroes.SpawnHeros(town.MapTile);
            }
        }
    }

}
