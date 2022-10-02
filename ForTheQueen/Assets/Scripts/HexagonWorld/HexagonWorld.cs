using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexagonWorld : SaveableMonoBehaviour, INavigatable<Vector2Int, Vector2Int>
{

    //idea: For the king mix with legends of andor bord game, with maybe settlement management from mount and blade
    //characters also have mount slot: map movespeed, combat accuracy penalties? mounted weapons?

    public const int WORLD_WIDTH = 100;

    public const int WORLD_HEIGHT = 100;

    public const float SPACE_BETWEEN_HEXES = 0f;

    public const float HEX_DIAMETER = 10f;

    public static readonly float HEX_Y_SPACING = Mathf.Sin(Mathf.Deg2Rad * 60) * HEX_DIAMETER;
    public static readonly float HEX_X_SPACING = Mathf.Cos(Mathf.Deg2Rad * 60) * HEX_DIAMETER * 1.5f;

    public const float HEX_RADIUS = HEX_DIAMETER / 2;

    public const float HEX_SMALL_RADIUS = HEX_RADIUS / 1.5f;

    public const float HEX_SMALL_SQR_RADIUS = HEX_SMALL_RADIUS * HEX_SMALL_RADIUS;

    public const float TOTAL_HEX_SPACE = SPACE_BETWEEN_HEXES + HEX_DIAMETER;


    public static TileBiom[] WORLD_BIOMS;

    [Save]
    protected int seed;

    [Save]
    protected WorldTile[,] world;

    protected int[,] isLand = new int[WORLD_WIDTH, WORLD_HEIGHT];

    public GameObject mapTileMarker;

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
        world = new WorldTile[WORLD_WIDTH, WORLD_HEIGHT];
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
        Assert.IsTrue(endX < WORLD_WIDTH);
        Assert.IsTrue(endY < WORLD_HEIGHT);

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
                WorldTile t = world[x, z];
                if (t == null)
                    continue;

                t.AddTileToMesh(verts, tris, colors);
                t.MarkTile(mapTileMarker, transform);
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

    protected bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < WORLD_WIDTH && point.y < WORLD_HEIGHT;
    }

    protected bool FieldCanBeEntered(Vector2Int point)
    {
        return WorldTileFromIndex(point).CanBeEntered;
    }

    #region Pathfinder

    public Vector2Int[] GetCircumjacent(Vector2Int field)
    {
        ///if a field cant be crossed (but may still be entered, e.g. enemies on map)
        ///no neighbours will be available for this field
        if (!world[field.x, field.y].CanBeCrossed)
            return Array.Empty<Vector2Int>();

        int x = field.x;
        int y = field.y;
        int sign;
        if (x % 2 == 0)
            sign = 1;
        else
            sign = -1;

        List<Vector2Int> neighbours = new List<Vector2Int>();
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x, y + 1));
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x, y - 1));
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x + 1, y + sign));
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x - 1, y + sign));
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x + 1, y));
        AddIfFieldIsAvailable(neighbours, new Vector2Int(x - 1, y));

        return neighbours.ToArray();
    }

    protected void AddIfFieldIsAvailable(List<Vector2Int> list, Vector2Int field)
    {
        if (IsFieldAvailableForPathfinder(field))
            list.Add(field);
    }

    public float DistanceToTarget(Vector2Int from, Vector2Int to)
    {
        int xDiff = Mathf.Abs(from.x - to.x);
        int yDiff = Mathf.Max(0, Mathf.Abs(from.y - to.y) - xDiff / 2) - (xDiff % 2);
        return xDiff + yDiff;
    }

    public float DistanceToField(Vector2Int from, Vector2Int to)
    {
        return world[to.x, to.y].biom.moveSpeedOnTile;
    }

    public bool ReachedTarget(Vector2Int current, Vector2Int destination)
    {
        return current == destination;
    }

    public bool IsEqual(Vector2Int t1, Vector2Int t2)
    {
        return t1.Equals(t2);
    }

    protected bool IsFieldAvailableForPathfinder(Vector2Int point)
    {
        return IsInBounds(point) && FieldCanBeEntered(point);
    }


    #endregion Pathfinder

    public Maybe<WorldTile> GetWorldTileFromPosition(Vector3 pos)
    {
        Vector2Int guessedIndex = GetNaiveArrayIndexFromPos(pos);
        List<Vector2Int> neighbours = GetUnfilteredCircumjacent(guessedIndex);
        neighbours.Add(guessedIndex);
        Maybe<Vector2Int> foundIndex = new Maybe<Vector2Int>();
        string result = "";
        foreach (var index in neighbours)
        {
            if(IsInBounds(index) && IsPointInsideHexagonSimple(index, pos))
            {
                if(foundIndex.HasValue)
                {
                    Debug.LogWarning("Mouse hovered detected over multiple fields. This should not happen.");
                }
                foundIndex.Value = index;
                result += $"Mouse hovered over index {index} and ";
            }
        }
        Debug.Log(result);
        return foundIndex.ApplyValueToFunction(WorldTileFromIndex);
    }

    protected WorldTile WorldTileFromIndex(Vector2Int index) => world[index.x, index.y];    

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
        Vector3 relativePos = point - WorldTileFromIndex(coord).CenterPos;
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


    public List<Vector2Int> GetUnfilteredCircumjacent(Vector2Int field)
    {
        int x = field.x;
        int y = field.y;
        int sign;
        if (x % 2 == 0)
            sign = 1;
        else
            sign = -1;
        List<Vector2Int> neighbours = new List<Vector2Int>() {
            new Vector2Int(x, y + 1),
            new Vector2Int(x, y - 1),
            new Vector2Int(x + 1, y + sign),
            new Vector2Int(x - 1, y + sign),
            new Vector2Int(x + 1, y),
            new Vector2Int(x - 1, y)
        };
        return neighbours;
    }


}
