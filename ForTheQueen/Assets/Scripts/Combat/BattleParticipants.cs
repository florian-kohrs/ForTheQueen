using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleParticipants
{

    public static int MAX_COMBAT_SIZE = 3;

    public BattleParticipants() { }

    public BattleParticipants(Vector2Int startTileCoord) :
      this(HexagonWorld.MapTileFromIndex(startTileCoord)) { }

    public BattleParticipants(MapTile startTile) : 
        this(startTile, 
            HexagonWorld.instance.MapTilesFromIndices(
                HexagonPathfinder.GetAccessableNeighboursInDistance(startTile.Coordinates, startTile.kingdomOfMapTile.KingdomBiom.fightAssistRange, false)))
    { }


    public List<IBattleOccupation> onPlayersSide;

    public List<IBattleOccupation> onEnemiesSide;


    public BattleParticipants(MapTile startTile, IEnumerable<MapTile> battleTiles)
    {
        onPlayersSide = new List<IBattleOccupation>();
        onEnemiesSide = new List<IBattleOccupation>();
        CheckOccupationForParticipants(startTile.Occupations, true);
        foreach (var item in battleTiles)
        {
            CheckOccupationForParticipants(item.Occupations, false);
        }
        while(onPlayersSide.Count > MAX_COMBAT_SIZE)
        {
            Debug.Log("Too many participants for player. Removing");
            onPlayersSide.RemoveAt(0);
        }

        while (onEnemiesSide.Count > MAX_COMBAT_SIZE)
        {
            Debug.Log("Too many participants for enemies. Removing");
            onEnemiesSide.RemoveAt(0);
        }
    }

    public void MarkBattleParticipants(MarkerMapping mapping)
    {
        HexagonMarker.Instance.MarkHexagons(onPlayersSide.Select(e => e.MapTile), HexagonMarker.Instance.battleParticipantMarker);
        foreach (var item in mapping.newTiles)
        {
            item.CurrentMarkerOnMapTile.GetComponent<Image>().color = Color.green;
        }

        HexagonMarker.Instance.MarkHexagons(onEnemiesSide.Select(e => e.MapTile), HexagonMarker.Instance.battleParticipantMarker);
        foreach (var item in mapping.newTiles)
        {
            item.CurrentMarkerOnMapTile.GetComponent<Image>().color = Color.red;
        }
    }

    protected void CheckOccupationForParticipants(IEnumerable<ITileOccupation> occs, bool mustParticipate)
    {
        foreach (var occ in occs)
        {
            if(occ is IBattleOccupation b && (mustParticipate || b.HelpsInFight))
            {
                if (b.OnPlayersSide)
                    onPlayersSide.Add(b);
                else
                    onEnemiesSide.Add(b);   

            }
        }
    }

}
