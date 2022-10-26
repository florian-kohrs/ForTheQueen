using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class HexagonWorld : HexagonGrid<MapTile>
{

    //idea: For the king mix with legends of andor bord game, with maybe settlement management from mount and blade
    //characters also have mount slot: map movespeed, combat accuracy penalties? mounted weapons?

    public const int WORLD_WIDTH = 100;

    public const int WORLD_HEIGHT = 100;

    public const float TILE_IS_WATER_BELOW_VALUE = 40;

    public Transform mapOccupationParent;

    public static MapTile[,] World => GameInstanceData.CurrentGameInstanceData.worldData.world;

    public static List<Continent> Continents => GameInstanceData.CurrentGameInstanceData.worldData.contintents;

    public static WorldData WorldData => GameInstanceData.CurrentGameInstanceData.worldData;

    protected bool IsWorldEmpty => Continents.Count == 0;

    protected int[,] isLand = new int[WORLD_WIDTH, WORLD_HEIGHT];

    public GameObject mapTileMarker;

    public Transform tileMarkerParent;

    public static HexagonWorld instance;

    public List<Action> onWorldCreated = new List<Action>();

    public static float GetWorldTotalWidth => instance.WorldTotalWidth;
    public static float GetWorldTotalHeigth => instance.WorldTotalHeight;

    private void Awake()
    {
        instance = this;
    }

    public static MapTile MapTileFromIndex(Vector2Int v2)
    {
        return World[v2.x, v2.y];
    }

    public static MapTile MapTileFromIndex(int x, int y)
    {
        return World[x, y];
    }


    public HexagonMarker marker;

    [SerializeField]
    protected MouseWorldEvents mouseMapHover;

    public AssetPolyRef<TileBiom>[] saveableBioms;


    public override Vector2Int Size => new Vector2Int(WORLD_WIDTH, WORLD_HEIGHT);

    public override MapTile[,] GridData => World;


    //protected TileBiom[] bioms;

    //public TileBiom[] Bioms
    //{
    //    get
    //    {
    //        if (bioms == null)
    //        {
    //            bioms = new TileBiom[saveableBioms.Length];
    //            for (int i = 0; i < saveableBioms.Length; i++)
    //            {
    //                bioms[i] = saveableBioms[i].RuntimeRef;
    //            }
    //        }
    //        return bioms;
    //    }
    //}

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

    private void Start()
    {
        if(IsWorldEmpty)
        {
            if(GameInstanceData.CurrentGameInstanceData.StartSeed == 0)
                GameInstanceData.CurrentGameInstanceData.ShuffleGameInstanceSeed();
            CreateWorld();
        }
        else
        {
            LoadSavedWorld();
        }
    }

    protected void LoadSavedWorld()
    {
        if (!PhotonNetworkExtension.IsMasterClient)
            return;

        byte[] gameData = GamePersistence.ObjectToByteArray(GameInstanceData.CurrentGameInstanceData);
        object[] gameParam = PhotonNetworkExtension.ToObjectArray(gameData);
        Broadcast.SafeRPC(photonView,
            nameof(LoadGame),
            RpcTarget.All,
            () => LoadGame(gameParam),
            gameParam);
    }

    [PunRPC]
    public void LoadGame(object[] gameDataO)
    {
        byte[] gameDataB = PhotonNetworkExtension.FromObjectArray<byte>(gameDataO);
        LoadGame(GamePersistence.ByteArrayToObject<GameInstanceData>(gameDataB));
    }

    protected void LoadGame(GameInstanceData gameInstance)
    {
        if(!PhotonNetwork.IsMasterClient)
            GameInstanceData.SetCurrentGameInstanceData(gameInstance);
        BuildWorld();
    }

    protected void BuildWorld()
    {
        BuildWorldMeshAndMapObjects();
        onWorldCreated.ForEach(x => x());
    }

    protected void InitializeWorld()
    {
        WorldData.world = new MapTile[WORLD_WIDTH, WORLD_HEIGHT];
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                MapTile t = new MapTile(new Vector2Int(x, y), this);
                World[x, y] = t;
            }
        }
    }

    public void CreateWorld()
    {
        if (!PhotonNetworkExtension.IsMasterClient)
            return;

        Broadcast.SafeRPC(photonView, 
            nameof(CreateWorldWithSeed), 
            RpcTarget.All, 
            CreateWorldWithSeed);
    }

    [PunRPC]
    public void CreateWorldWithSeed()
    {
        InitializeWorld();
        CreateContinentAt(new Vector2Int(5,5), new Vector2Int(50, 50));
        BuildWorld();
        GameInstanceData.CurrentGameInstanceData.timeData.StartGame();
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

        Continent continent = new Continent(this, GameInstanceData.Rand.Next(), saveableBioms, startPos, size, distanceNoiseWeighting);
        Continents.Add(continent);
    }


    protected void BuildWorldMeshAndMapObjects()
    {
        Mesh m = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Color> colors = new List<Color>();

        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int z = 0; z < WORLD_HEIGHT; z++)
            {
                MapTile t = World[x, z];
                t.AddTileToMesh(verts, tris, colors);
                t.MarkTile(mapTileMarker, tileMarkerParent);
                t.SpawnAllTileOccupations(mapOccupationParent);
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

    public static new bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.y >= 0 && point.x < WORLD_WIDTH && point.y < WORLD_HEIGHT;
    }


    //public bool IsPointInsideHexagon(Vector3 point)
    //{
    //    Vector3 relative = point - CenterPos;
    //    bool isInside = relative.x > Left.x && relative.x < Right.x;
    //    isInside &= relative.y > BotLeft.y && relative.y < TopLeft.y;

    //}

    //protected bool IsPointInside


 

}
