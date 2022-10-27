using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseMapMovement<Agent, MapTileType> : MonoBehaviourPun, IMouseTileSelectionCallback<MapTileType> 
    where Agent : IMovementAgent where MapTileType : BaseMapTile
{

    protected abstract Agent CurrentAgent { get; }

    public int RestMovementInTurn => CurrentAgent.MovementRemaining;

    public abstract void RegisterMouseCallback();
    public abstract void UnregisterMouseCallback();

    protected List<Vector2Int> pathToCurrentHovoredTile;


    protected MapTileType currentHoveredTile;

    protected bool isInAnimation;

    protected MarkerMapping mapMovementMarker;

    protected virtual bool IsCurrentHovoredTileInRange => pathToCurrentHovoredTile.Count <= RestMovementInTurn;

    protected abstract HexagonGrid<MapTileType> Grid { get; }

    protected virtual Func<HexagonPathfinder, Vector2Int, Vector2Int, bool> ReachTargetOverride => null;

    [PunRPC]
    public void BeginTileHoverRPC(int x, int y)
    {
        Vector2Int coord = new Vector2Int(x, y);
        MapTileType mapTile = Grid.DataFromIndex(coord);
        if (!GameManager.AllowPlayerMovement || !mapTile.IsValidMovementTarget(CurrentAgent.CanEnterWater))
            return;

        currentHoveredTile = mapTile;
        DisplayPath(CurrentAgent.CurrentTile, mapTile.Coordinates);
    }

    protected virtual void OnBeginTileHover(Vector2Int coord) { }

    protected void ClearPathDisplay()
    {
        if (mapMovementMarker == null)
        {
            Debug.LogWarning("Map markers are null. is that important?");
            return;
        }

        mapMovementMarker.ClearMarkers();
    }


    public void DisplayPath(Vector2Int start, Vector2Int end)
    {
        ClearPathDisplay();
        pathToCurrentHovoredTile = HexagonPathfinder.GetPath(Grid, start, end, false, PathAccuracy.Perfect, ReachTargetOverride);
        mapMovementMarker = HexagonMarker.Instance.MarkHexagons(Grid, pathToCurrentHovoredTile, HexagonMarker.Instance.mapMovementMarker);

        int count = 1;
        foreach (GameObject marker in mapMovementMarker.markerMapping.Values)
        {
            MovementMapMarker m = marker.GetComponent<MovementMapMarker>();

            Color displayColor;
            if (count <= RestMovementInTurn)
            {
                displayColor = Color.green;
                m.distanceText.text = (RestMovementInTurn - count).ToString();
            }
            else
            {
                m.distanceText.text = string.Empty;
                displayColor = Color.gray;
            }

            displayColor.a = 0.7f;
            m.hexagonImage.color = displayColor;

            count++;
        }
    }

    public void BeginTileHover(MapTileType tile)
    {
        if (isInAnimation)
            return;
        int x = tile.Coordinates.x;
        int y = tile.Coordinates.y;
        Broadcast.SafeRPC(photonView, nameof(BeginTileHoverRPC), RpcTarget.All, ()=>BeginTileHoverRPC(x,y), x,y);
        OnBeginTileHover(tile.Coordinates);
    }



    [PunRPC]
    public void MoveTowardsClickedTile(object[] pathParam)
    {
        Vector2Int[] path = PhotonNetworkExtension.FromObjectArray<Vector2>(pathParam).ToVector2IntArray();
        AnimateMovement(path);
    }

    public abstract void AnimateMovement(Vector2Int[] path);

    public void OnMouseStay(MapTileType tile)
    {
    }

    public void ExitTileHovered(MapTileType tile)
    {
        if (isInAnimation)
            return;
        currentHoveredTile = null;
        ClearPathDisplay();
    }

    protected void OnEndMovement()
    {
        isInAnimation = false;
        ExitTileHovered(currentHoveredTile);
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentHoveredTile != null && IsCurrentHovoredTileInRange && GameManager.AllowPlayerMovement)
        {
            OnClickOnValidField(currentHoveredTile.Coordinates);
        }
    }

    protected virtual void OnClickOnValidField(Vector2Int v2)
    {
        isInAnimation = true;
        object[] pathParam = PhotonNetworkExtension.ToObjectArray(VectorExtension.ToVector2Array(pathToCurrentHovoredTile));
        Broadcast.SafeRPC(
            photonView,
            nameof(MoveTowardsClickedTile),
            RpcTarget.All,
            () => MoveTowardsClickedTile(pathParam),
            pathParam);
    }

    private void Start()
    {
        RegisterMouseCallback();
    }


    private void OnDestroy()
    {
        UnregisterMouseCallback();
        ClearPathDisplay();
    }

}
