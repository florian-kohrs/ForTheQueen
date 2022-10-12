using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseEnemyOccupation<T> : TileInteractableOccupation<T>, IBaseEnemyOccupation where T : BaseEnemyOccupationScriptableObject
{

    public BaseEnemyOccupation() { }

    public BaseEnemyOccupation(T e) : base(e) { }


    protected float DespawnTime => 0.4f * GameManager.AnimationSpeed;

    public bool OnPlayersSide => false;

    public abstract bool HasSupportRange { get; }

    public abstract bool HelpsInFight { get; }

    protected MarkerMapping battleMarkers;

    public void Despawn()
    {
        Delete();
    }

    public void Delete()
    {
        MapTile.RemoveTileOccupation(this);
        GameObject.Destroy(mapOccupationInstance);
    }


    public override void OnPlayerEntered(Hero p)
    {
        OnPlayerMouseExit(p);
        InterfaceController.GetInterfaceMask<PreBattleUI>().AdaptUIAndOpen(new BattleParticipants(MapTile), mapTile.CenterPos);
        //TODO: Open Fight UI
    }

    public override void OnPlayerMouseExit(Hero p)
    {
        battleMarkers.ClearMarkers();
        InterfaceController.Instance.RemoveMask<GenericMouseHoverInfo>();
    }

    public override void OnPlayerMouseHover(Hero p)
    {
        IEnumerable<Vector2Int> neighbours = HexagonPathfinder.GetAccessableNeighboursInDistance(MapTile.Coordinates, MapTile.kingdomOfMapTile.KingdomBiom.fightAssistRange, false);
        IEnumerable<MapTile> tiles = HexagonWorld.MapTilesFromIndices(neighbours);

        battleMarkers = HexagonMarker.Instance.MarkHexagons(tiles, HexagonMarker.Instance.battleParticipantMarker);
        BattleParticipants b = new BattleParticipants(MapTile, tiles);
        HexagonMarker.Instance.MarkHexagons(tiles, HexagonMarker.Instance.battleParticipantMarker);

        InterfaceController.GetInterfaceMask<GenericMouseHoverInfo>().AdaptUIAndOpen(OccupationObject, mapTile.CenterPos);
    }

    public override void OnPlayerReachedFieldAsTarget(Hero p)
    {
    }

    public override void OnPlayerUncovered(Hero p)
    {
    }

    public abstract void DisplayInPreFight(Transform parent);
}
