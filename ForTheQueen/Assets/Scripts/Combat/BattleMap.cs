using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap : MonoBehaviour
{

    public const int CENTER_AREA = 2;

    public Vector2Int battleMapSize;

    public int biomIndex;

    [SerializeField]
    protected Transform parent;

    [SerializeField]
    protected GameObject battleMapTilePrefab;

    protected IBattleParticipant[,] battleMapParticipant;

    //TODO: Let players choose their start position (save selection)
    public void BeginBattle(BattleParticipants participants)
    {
        CreateMap();

        battleMapParticipant = new IBattleParticipant[battleMapSize.x, battleMapSize.y];

        int i = 0;
        foreach (var item in participants.onPlayersSide)
        {
            SetParticipantAt(battleMapSize.y - 2, 1 + i, item);
            i++;
        }

        i = 0;
        foreach (var item in participants.onEnemiesSide)
        {
            SetParticipantAt(1, 1 + i, item);
            i++;
        }
    }

    protected void SetParticipantAt(int x, int z, IBattleOccupation occ)
    {
        SetParticipantAt(x,z,occ.GetParticipant());
    }

    protected void SetParticipantAt(int x, int z, IBattleParticipant part)
    {
        GameObject o = part.SpawnCombatObject();
        o.transform.position = GetCenterPos(x, z);
        battleMapParticipant[x, z] = part;
    }

    protected void CreateMap()
    {

        for (int x = 0; x < battleMapSize.x; x++)
        {
            for (int y = 0; y < battleMapSize.y; y++)
            {
                SpawnTileAt(x, y);
            }
        }
    }

    protected void SpawnTileAt(int x, int z)
    {
        GameObject t = Instantiate(battleMapTilePrefab, parent);
        t.transform.position = MapTile.GetPosForCoord(new Vector2Int(x, z));
    }

    public Vector3 GetCenterPos(int x, int z) => MapTile.GetPosForCoord(new Vector2Int(x, z));

}
