using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Continent 
{

    public string ContinentName;

    public Vector2Int startCoord;

    public Vector2Int size;

    protected Vector2Int offset;

    protected const int MIN_DISTANCE_BETWEEN_TOWNS = 8;

    /// <summary>
    /// determines how much the noise value at position influences choice if tile is water or land
    /// </summary>
    protected AnimationCurve distanceNoiseWeighting;

    protected int[,] continentFactionAssignment;

    public Dictionary<TileBiom, HashSet<MapTile>> mapTilesPerKingdom = new Dictionary<TileBiom, HashSet<MapTile>>();

    protected TileBiom[] kingdomsOnContinent;

    protected HexagonWorld world;

    public Continent(HexagonWorld world, TileBiom[] kingdomsOnContinent, Vector2Int startCoord, Vector2Int size, AnimationCurve distanceNoiseWeighting)
    {
        this.world = world;
        this.kingdomsOnContinent = kingdomsOnContinent;
        this.startCoord = startCoord;
        this.size = size;
        offset = default;// new Vector2Int(Random.Range(0, 9999), Random.Range(0, 99999));
        this.distanceNoiseWeighting = distanceNoiseWeighting;
        continentFactionAssignment = new int[size.x, size.y];
        BuildContinent();
    }

    protected void AddTileToDict(MapTile t, int factionIndex)
    {
        HashSet<MapTile> tiles;
        TileBiom b = kingdomsOnContinent[factionIndex - 1];
        if (!mapTilesPerKingdom.TryGetValue(b, out tiles))
        {
            tiles = new HashSet<MapTile>();
            mapTilesPerKingdom[b] = tiles;
        }
        tiles.Add(t);
    }

    public void SpawnObjectsForAllKingdoms()
    {
        for (int i = 0; i < kingdomsOnContinent.Length; i++)
        {
            SpawnObjectsForKingdom(kingdomsOnContinent[i]);
        }
    }

    protected void SpawnObjectsForKingdom(TileBiom biom)
    {
        HashSet<MapTile> kingdomTiles = new HashSet<MapTile>(mapTilesPerKingdom[biom]);
        
        foreach (var townObject in biom.townsInBiom)
        {
            MapTile t = Rand.PickOne(kingdomTiles);
            Town town = new Town(townObject);
            t.AddAndSpawnTileOccupation(town, world.transform);
            foreach (var item in world.MapTilesFromIndices(HexagonPathfinder.GetNeighboursInDistance(t.Coordinates, MIN_DISTANCE_BETWEEN_TOWNS)))
            {
                kingdomTiles.Remove(item);
            }
        }
    }


    public void WriteContinentFactionTilesIntoWorld(MapTile[,] world)
    {
        for (int x = 0; x < size.x; x++)
        {
            int currentX = startCoord.x + x;
            for (int y = 0; y < size.y; y++)
            {
                int currentKingdomIndex = continentFactionAssignment[x, y];
                if (currentKingdomIndex == 0)
                    continue;

                float noise = NoiseForPoint(x, y);
                if (noise < HexagonWorld.TILE_IS_WATER_BELOW_VALUE)
                    continue;

                int currentY = startCoord.y + y;
                world[currentX, currentY].biomIndex = currentKingdomIndex;
                AddTileToDict(world[currentX, currentY], currentKingdomIndex);
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
