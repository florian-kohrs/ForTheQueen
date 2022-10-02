using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexagonWorld : SaveableMonoBehaviour
{

    //idea: For the king mix with legends of andor bord game, with maybe settlement management from mount and blade
    //characters also have mount slot: map movespeed, combat accuracy penalties? mounted weapons?

    public const int WORLD_WIDTH = 50;

    public const int WORLD_HEIGHT = 50;

    public const float SPACE_BETWEEN_HEXES = 0f;

    public const float HEX_DIAMETER = 10f;

    public static readonly float HEX_Y_SPACING = Mathf.Sin(Mathf.Deg2Rad * 60) * HEX_DIAMETER;
    public static readonly float HEX_X_SPACING = Mathf.Cos(Mathf.Deg2Rad * 60) * HEX_DIAMETER * 1.5f;

    public const float HEX_RADIUS = HEX_DIAMETER / 2;

    public static readonly float HEX_SMALL_SQR_RADIUS = Mathf.Pow(HEX_Y_SPACING / 2,2);

    public const float TOTAL_HEX_SPACE = SPACE_BETWEEN_HEXES + HEX_DIAMETER;

    public static TileBiom[] WORLD_BIOMS;

    [Save]
    protected int seed;

    [Save]
    protected MapTile[,] world;

    public MapTile[,] World => world;

    protected int[,] isLand = new int[WORLD_WIDTH, WORLD_HEIGHT];

    public GameObject mapTileMarker;

    public Transform tileMarkerParent;

    public HexagonPathfinder pathfinder;

    public HexagonMarker marker;

    [SerializeField]
    protected MouseToHoveredMapTile mouseMapHover;

    public TileBiom[] bioms;

    [Tooltip("Determines the influence of noise of the tile based on the distance to the border. " +
        "1 is full influence 0 is no influence.")]
    public AnimationCurve distanceNoiseWeighting;


    protected MeshFilter filter;

    protected MeshFilter Filter
    {
        get
        {
            if(filter == null)  
                filter = GetComponent<MeshFilter>();
            return filter;
        }
    }

    protected MeshRenderer meshRenderer;

    protected MeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            return meshRenderer;
        }
    }

    public CallbackSet<IMouseTileSelectionCallback> GetMouseCallbackSet => mouseMapHover.subscribers;

    protected MeshCollider meshCollider;

    protected MeshCollider MeshCollider
    {
        get
        {
            if (meshCollider == null)
                meshCollider = GetComponent<MeshCollider>();
            return meshCollider;
        }
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        seed = UnityEngine.Random.Range(0, 99999999);
        world = new MapTile[WORLD_WIDTH, WORLD_HEIGHT];
    }

    private void Start()
    {
        WORLD_BIOMS = bioms;
        CreateWorld();
    }

    //sprite for hexagon markings

    public void CreateWorld()
    {
        CreateContinentAt(new Vector2Int(0, 0), new Vector2Int(50, 50));
        BuildWorldMesh();
    }

    protected int GetBiomAt(int x, int y)
    {
        x -= 50;
        y -= 50;
        if (x * x + y * y < 50)
        {
            return 0;
        }
        else return 1;
    }

    protected void CreateContinentAt(Vector2Int startPos, Vector2Int size)
    {
        int endX = startPos.x + size.x;
        int endY = startPos.y + size.y;
        Assert.IsTrue(endX <= WORLD_WIDTH);
        Assert.IsTrue(endY <= WORLD_HEIGHT);

        Continent continent = new Continent(startPos, size, distanceNoiseWeighting);
        continent.WriteContinentFactionTilesIntoWorld(world);
    }


    protected void BuildWorldMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Color> colors = new List<Color>();

        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int z = 0; z < WORLD_HEIGHT; z++)
            {
                MapTile t = world[x, z];
                if (t == null)
                    continue;

                t.AddTileToMesh(verts, tris, colors);
                t.MarkTile(mapTileMarker, tileMarkerParent);
            }
        }

        m.vertices = verts.ToArray();
        m.triangles = tris.ToArray();
        m.colors = colors.ToArray();
        m.RecalculateNormals();
        ApplyMesh(m);
    }

    protected Mesh BuildMeshOfContinent()
    {
        return null;
    }

    protected void ApplyMesh(Mesh m)
    {
        Filter.mesh = m;
        MeshCollider.sharedMesh = m;
    }

    public List<GameObject> MarkHexagons(IEnumerable<Vector2Int> coords, GameObject markerPrefab = null)
    {
        return MarkHexagons(MapTilesFromIndices(coords), markerPrefab);
    }

    public List<GameObject> MarkHexagons(IEnumerable<MapTile> tiles, GameObject markerPrefab = null)
    {
        return marker.MarkHexagons(tiles, markerPrefab);
    }

    public bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < WORLD_WIDTH && point.y < WORLD_HEIGHT;
    }

    public bool FieldCanBeEntered(Vector2Int point)
    {
        return MapTileFromIndex(point).CanBeEntered;
    }

    public IEnumerable<MapTile> GetAdjencentTiles(Vector2Int coord, bool includeCenter = false)
    {
        return GetAdjencentTiles(MapTileFromIndex(coord),includeCenter);
    }

    public IEnumerable<MapTile> GetAdjencentTiles(MapTile center, bool includeCenter = false)
    {
        if (includeCenter)
            yield return center;

        foreach (MapTile tile in MapTilesFromIndices(GetInBoundsNeighbours(center.Coordinates)))
            yield return tile;
    }

    public Maybe<MapTile> GetWorldTileFromPosition(Vector3 pos)
    {
        Vector2Int guessedIndex = GetNaiveArrayIndexFromPos(pos);
        List<Vector2Int> neighbours = GetInBoundsNeighbours(guessedIndex);
        neighbours.Add(guessedIndex);
        Maybe<Vector2Int> foundIndex = new Maybe<Vector2Int>();
        foreach (var index in neighbours)
        {
            if(IsInBounds(index) && IsPointInsideHexagonSimple(index, pos))
            {
                if(foundIndex.HasValue)
                {
                    Debug.LogWarning("Mouse hovered detected over multiple fields. This should not happen.");
                }
                foundIndex.Value = index;
            }
        }
        return foundIndex.ApplyValueToFunction(MapTileFromIndex);
    }

    public MapTile MapTileFromIndex(Vector2Int index) => world[index.x, index.y];    

    public IEnumerable<MapTile> MapTilesFromIndices(IEnumerable<Vector2Int> indices)
    {
        return indices.Select(MapTileFromIndex);
    }

    protected Vector2Int GetNaiveArrayIndexFromPos(Vector3 pos)
    {
        //pos.x -= HEX_RADIUS;
        //pos.z -= HEX_RADIUS;
        int coordX = (int)(pos.x / (HexagonWorld.HEX_X_SPACING + HexagonWorld.SPACE_BETWEEN_HEXES));

        if (coordX % 2 != 0)
            pos.z -= HexagonWorld.HEX_Y_SPACING / 2;

        int coordY = (int)(pos.z / (HexagonWorld.HEX_Y_SPACING + HexagonWorld.SPACE_BETWEEN_HEXES));

        return new Vector2Int(coordX,coordY);
    }

    /// <summary>
    /// checks if distance is within circle lying inside hexagon
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public bool IsPointInsideHexagonSimple(Vector2Int coord, Vector3 point)
    {
        Vector3 relativePos = point - MapTileFromIndex(coord).CenterPos;
        float sqrDist = Vector3.SqrMagnitude(relativePos);
        return sqrDist < HEX_SMALL_SQR_RADIUS;
    }

    //public bool IsPointInsideHexagon(Vector3 point)
    //{
    //    Vector3 relative = point - CenterPos;
    //    bool isInside = relative.x > Left.x && relative.x < Right.x;
    //    isInside &= relative.y > BotLeft.y && relative.y < TopLeft.y;

    //}

    //protected bool IsPointInside


    public List<Vector2Int> GetInBoundsNeighbours(Vector2Int field)
    {
        return pathfinder.GetCircumjacent(field, IsInBounds).ToList();
    }


}