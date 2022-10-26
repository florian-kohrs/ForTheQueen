using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleMap : HexagonGrid<BattleMapTile>
{

    public const int CENTER_AREA = 2;

    public Vector2Int battleMapSize;

    public int biomIndex;

    [SerializeField]
    protected Transform parent;

    [SerializeField]
    protected GameObject battleMapTilePrefab;

    protected BattleMapTile[,] battleMapContent;

    public override Vector2Int Size => battleMapSize;

    public override BattleMapTile[,] GridData => battleMapContent;

    public List<IBattleParticipant> activeParticipants = new List<IBattleParticipant>();


    protected List<GameObject> currentMapMarkers = new List<GameObject>();

    public Transform mapMarkerParent;

    public GameObject mapMarkerPrefab;

    public float mapMarkerScale;

    public void MarkActionOnMap(Vector2Int v2, CombatAction action)
    {
        RemovePreviousMarkers();
        foreach (var v in action.GetTargetFieldsFromAction(HeroCombat.currentHeroTurnInCombat.CurrentTile, v2, action.target, IsInBounds))
            AddCurrentMarkerAt(v, !action.targetEnemies);
    }

    protected void AddCurrentMarkerAt(Vector2Int v2, bool actionTargetSelf)
    {
        GameObject marker = Instantiate(mapMarkerPrefab, mapMarkerParent);
        marker.transform.localScale = new Vector3(mapMarkerScale, mapMarkerScale, mapMarkerScale);
        currentMapMarkers.Add(marker);
        BattleMapTile part = GridData[v2.x, v2.y];
        marker.transform.position = part.CenterPos + Vector3.up * 0.001f;
        if(part.participant != null)
        {
            marker.GetComponent<Image>().color = actionTargetSelf ? Color.green : Color.red;
        }
    }


    public IEnumerable<IBattleParticipant> GetAfflictedParticipants(Vector2Int selectedField, CombatAction a)
    {
        return a.GetTargetFieldsFromAction(selectedField, selectedField, a.target, IsInBounds).
            Select(v2 => DataFromIndex(v2).participant).
            Where(b => b != null);
    }


    public void RemovePreviousMarkers()
    {
        foreach (var item in currentMapMarkers)
        {
            Destroy(item);
        }
        currentMapMarkers.Clear();
    }


    //TODO: Let players choose their start position (save selection)
    public ICollection<IBattleParticipant> BeginBattle(BattleParticipants participants)
    {
        CreateMap();

        int i = 0;
        foreach (var item in participants.onPlayersSide)
        {
            Transform t = SetParticipantAt(1 + i, battleMapSize.y - 2, item);
            t.Rotate(0, 180, 0);
            i++;
        }

        i = 0;
        foreach (var item in participants.onEnemiesSide)
        {
            SetParticipantAt(1 + i, 1, item);
            i++;
        }

        List<IBattleParticipant> ps = new List<IBattleParticipant>();
        foreach (var item in battleMapContent)
        {
            if (item.participant != null)
                ps.Add(item.participant);
        }
        return ps;
    }



    protected Transform SetParticipantAt(int x, int z, IBattleOccupation occ)
    {
        return SetParticipantAt(x,z,occ.GetParticipant());
    }

    protected Transform SetParticipantAt(int x, int z, IBattleParticipant part)
    {
        activeParticipants.Add(part);
        part.CurrentTile = new Vector2Int(x, z);
        part.gameObject.transform.position = GetCenterPos(x, z);
        battleMapContent[x, z].participant = part;
        return part.gameObject.transform;
    }

    protected void CreateMap()
    {
        battleMapContent = new BattleMapTile[battleMapSize.x, battleMapSize.y];

        for (int x = 0; x < battleMapSize.x; x++)
        {
            for (int y = 0; y < battleMapSize.y; y++)
            {
                battleMapContent[x, y] = new BattleMapTile(new Vector2Int(x, y), this);
                SpawnTileAt(x, y);
            }
        }
    }

    protected void SpawnTileAt(int x, int z)
    {
        GameObject t = Instantiate(battleMapTilePrefab, parent);
        t.transform.position = GetCenterPos(x, z);
    }

    public Vector3 GetCenterPos(int x, int z) => MapTile.GetCenterPosForCoord(new Vector2Int(x, z), SpaceBetweenHexes, HexXSpacing, HexYSpacing);

}
