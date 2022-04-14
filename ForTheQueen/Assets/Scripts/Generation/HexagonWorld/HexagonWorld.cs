using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexagonWorld : SaveableMonoBehaviour
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

    public const float TOTAL_HEX_SPACE = SPACE_BETWEEN_HEXES + HEX_DIAMETER;

    public static TileBiom[] WORLD_BIOMS;

    [Save]
    protected int seed;

    [Save]
    protected WorldTile[,] world;

    protected int[,] isLand = new int[WORLD_WIDTH, WORLD_HEIGHT];


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
        seed = Random.Range(0, 99999999);
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
        CreateContinentAt(new Vector2Int(10, 10), new Vector2Int(50, 50));
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
            }
        }

        m.vertices = verts.ToArray();
        m.triangles = tris.ToArray();
        m.colors = colors.ToArray();
        m.RecalculateNormals();
        ApplyMesh(m);
    }

    protected void ApplyMesh(Mesh m)
    {
        Filter.mesh = m;
        MeshCollider.sharedMesh = m;
    }

}
