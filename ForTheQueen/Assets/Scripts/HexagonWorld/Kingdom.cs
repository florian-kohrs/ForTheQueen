using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Kingdom
{

    public Kingdom() { }

    public Kingdom(AssetPolyRef<TileBiom> biom, HexagonWorld world)
    {
        this.world = world;
        this.biom = biom;
    }

    protected const int MIN_DISTANCE_BETWEEN_TOWNS = 8;


    public HashSet<MapTile> mapFieldsOfKingdom = new HashSet<MapTile>();

    [NonSerialized]
    public HexagonWorld world;

    protected AssetPolyRef<TileBiom> biom;

    public TileBiom KingdomBiom => biom.RuntimeRef;

    /// <summary>
    /// 
    /// </summary>
    /// <returns>returns all maptiles of the kingdom, that are free of reachable and free of occupations</returns>
    public void SpawnOccupationToKingdom(System.Random rand)
    {
        SpawnTowns(rand);
        SpawnPermanentOccupations(rand);
    }

    protected void SpawnTowns(System.Random rand)
    {
        HashSet<MapTile> townFieldOfKingdom = new HashSet<MapTile>(mapFieldsOfKingdom);

        List<MapTile> lastTowns = new List<MapTile>();

        foreach (var townObject in KingdomBiom.townsInBiom)
        {
            MapTile t = Rand.PickOne(townFieldOfKingdom, rand);
            Town town = new Town(townObject);
            t.AddTileOccupation(town);
            foreach (var item in world.MapTilesFromIndices(HexagonPathfinder.GetNeighboursInDistance(t.Coordinates, MIN_DISTANCE_BETWEEN_TOWNS)))
            {
                townFieldOfKingdom.Remove(item);
            }
            foreach (var lastTown in lastTowns)
            {
                PreventPathBlockageBetweenImportantSettlements(town.MapTile, lastTown);
            }
            if (town.OccupationObject.isStartTown)
            {
                Heroes.SpawnHeros(town.MapTile);
            }
            lastTowns.Add(town.mapTile);
        }
    }

    protected void SpawnPermanentOccupations(System.Random rand)
    {
        foreach (var tile in mapFieldsOfKingdom)
        {
            if(!tile.CanBePermanentlyOccupied)
                continue;

            float tileRand = rand.Next(0,100);
            if(tileRand <= KingdomBiom.blockingOccupationDensity)
            {
                SpawnOccupationAt(tile, rand);
            }
        }
    }

    protected void SpawnOccupationAt(MapTile tile, System.Random rand)
    {
        TileBlockingOccupationObject occupation = Rand.PickOne(KingdomBiom.blockingOccupations, rand);
        tile.AddTileOccupation(new TileBlockingOccupationInstance(occupation));
    }

    protected void PreventPathBlockageBetweenImportantSettlements(MapTile from, MapTile to)
    {
        IEnumerable<Vector2Int> path = Pathfinder<Vector2Int, Vector2Int>.FindPath(world.pathfinder, from.Coordinates, to.Coordinates, PathAccuracy.Decent);
        BlockTilesForMapOccupation(world.MapTilesFromIndices(path));
    }

    protected void BlockTilesForMapOccupation(IEnumerable<MapTile> mapTile)
    {
        foreach (var tile in mapTile)
            tile.canBePermanantlyOccupied = false;
    }

}
