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
    public ICollection<IBattleParticipant> BeginBattle(BattleParticipants participants)
    {
        CreateMap();
        battleMapParticipant = new IBattleParticipant[battleMapSize.x, battleMapSize.y];

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
        foreach (var item in battleMapParticipant)
        {
            if (item != null)
                ps.Add(item);
        }
        return ps;
    }

    protected Transform SetParticipantAt(int x, int z, IBattleOccupation occ)
    {
        return SetParticipantAt(x,z,occ.GetParticipant());
    }

    protected Transform SetParticipantAt(int x, int z, IBattleParticipant part)
    {
        part.gameObject.transform.position = GetCenterPos(x, z);
        battleMapParticipant[x, z] = part;
        return part.gameObject.transform;
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
