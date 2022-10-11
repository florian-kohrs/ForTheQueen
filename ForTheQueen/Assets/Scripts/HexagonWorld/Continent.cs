using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Continent 
{

    public string continentName;

    public SerializableVector2Int startCoord;

    public SerializableVector2Int size;

    protected SerializableVector2Int offset;


    /// <summary>
    /// determines how much the noise value at position influences choice if tile is water or land
    /// </summary>
    [NonSerialized]
    protected AnimationCurve distanceNoiseWeighting;

    
    [HideInInspector]
    protected Kingdom[] kingdomsOnContinent;

    public Dictionary<TileBiom, HashSet<MapTile>> mapTilesPerKingdom = new Dictionary<TileBiom, HashSet<MapTile>>();

    [NonSerialized]
    protected HexagonWorld world;

    public Continent() { }

    public Continent(HexagonWorld world, int seed, AssetPolyRef<TileBiom>[] biomsOnContinent, Vector2Int startCoord, Vector2Int size, AnimationCurve distanceNoiseWeighting)
    {
        kingdomsOnContinent = new Kingdom[biomsOnContinent.Length];
        for (int i = 0; i < biomsOnContinent.Length; i++)
        {
            kingdomsOnContinent[i] = new Kingdom(biomsOnContinent[i], world);
        }

        System.Random rand = new System.Random(seed);
        this.world = world;
        this.startCoord = startCoord;
        this.size = size;
        offset = new Vector2Int(rand.Next(0,999999), rand.Next(0, 999999));
        this.distanceNoiseWeighting = distanceNoiseWeighting;
        int[,] continentFactionAssignment;

        BuildContinent(out continentFactionAssignment);
        WriteContinentFactionTilesIntoWorld(continentFactionAssignment);
        SpawnObjectsForAllKingdoms(rand);
    }

    protected void AddTileToDict(MapTile t, int factionIndex)
    {
        t.kingdomOfMapTile = kingdomsOnContinent[factionIndex];
        kingdomsOnContinent[factionIndex].mapFieldsOfKingdom.Add(t);
    }

    protected void RemoveTileFromDict(MapTile t)
    {
        t.kingdomOfMapTile.mapFieldsOfKingdom.Add(t);
    }

    public void SpawnObjectsForAllKingdoms(System.Random rand)
    {
        foreach (var kingdom in kingdomsOnContinent)
        {
            kingdom.SpawnOccupationToKingdom(rand);
        }
    }

    public void RespawnEnemies(System.Random rand)
    {
        foreach (var kingdom in kingdomsOnContinent)
        {
            kingdom.SpawnEnemies(rand);
        }
    }

    protected void DoForEachField(Action<MapTile> a)
    {
        for (int x = 0; x < size.x; x++)
        {
            int currentX = startCoord.x + x;
            for (int y = 0; y < size.y; y++)
            {
                int currentY = startCoord.y + y;
                a(HexagonWorld.MapTileFromIndex(currentX, currentY));
            }
        }
    }

    public void WriteContinentFactionTilesIntoWorld(int[,] continentFactionAssignment)
    {
        for (int x = 0; x < size.x; x++)
        {
            int globalX = startCoord.x + x;
            for (int y = 0; y < size.y; y++)
            {
                int currentKingdomIndex = continentFactionAssignment[x, y];
                float noise = NoiseForPoint(x, y);

                if (noise < HexagonWorld.TILE_IS_WATER_BELOW_VALUE)
                    continue;

                int globalY = startCoord.y + y;
                RegisterMapTile(currentKingdomIndex, globalX,globalY);
            }
        }
        RemoveIslands();
    }

    protected void RegisterMapTile(int currentKingdomIndex, int currentX, int currentY)
    {
        AddTileToDict(HexagonWorld.World[currentX, currentY], currentKingdomIndex);
    }

    protected void RemoveFromWorld(int currentX, int currentY)
    {
        RemoveTileFromDict(HexagonWorld.World[currentX, currentY]);
    }

    protected void RemoveIslands()
    {
        bool[,] landTiles = new bool[size.x, size.y];
        DjikstraFactionAssignment<bool>.BuildDjikstraOnMap(landTiles, System.Tuple.Create(true, new Vector2Int(size.x / 2, size.y/2)),
            (v2) => !HexagonWorld.MapTileFromIndex(v2).IsWater);

        for (int x = 0; x < size.x; x++)
        {
            int currentX = startCoord.x + x;
            for (int y = 0; y < size.y; y++)
            {
                int currentY = startCoord.y + y;
                if(landTiles[x, y])   
                    continue;
                MapTile t = HexagonWorld.MapTileFromIndex(currentX, currentY);
                if (t.IsWater)
                    continue;

                RemoveFromWorld(currentX, currentY);
            }
        }
    }

protected float NoiseForPoint(int x, int y)
    {
        float noiseScale = 0.1f;
        ///gets values between 0 and 0.5
        /// 0.5 if the value is in the center and 0 if its on the border
        float xDiff = 0.5f - Mathf.Abs(x - size.x / 2) / (float)size.x;
        float yDiff = 0.5f - Mathf.Abs(y - size.y / 2) / (float)size.y;

        x += offset.x;
        y += offset.y;
        float noise = SimplexNoise.Noise.CalcPixel2D(x, y, noiseScale);
        float centerMultiplier = distanceNoiseWeighting.Evaluate(Mathf.Min(xDiff, yDiff));
        ///make border of continent to water
        noise *= centerMultiplier;
        ///make center of continent to land
        noise += centerMultiplier * HexagonWorld.TILE_IS_WATER_BELOW_VALUE / 1.5f;
        return noise;
    }

    protected void BuildContinent(out int[,] continentFactionAssignment)
    {
        continentFactionAssignment = new int[size.x, size.y];
        List<Tuple<int, Vector2Int>> startPoints = new List<Tuple<int, Vector2Int>>()
        {
            Tuple.Create(0, new Vector2Int(2, 0)),
            Tuple.Create(1, new Vector2Int(size.x - 5, 0)),
            Tuple.Create(2, new Vector2Int(1, size.y - 1)),
            Tuple.Create(3, new Vector2Int(size.x - 1, size.y - 3))
        };
        DjikstraFactionAssignment<int>.BuildDjikstraOnMap(continentFactionAssignment, startPoints);
    }

}
