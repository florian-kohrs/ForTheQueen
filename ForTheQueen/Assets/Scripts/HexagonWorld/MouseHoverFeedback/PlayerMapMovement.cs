using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMapMovement : MonoBehaviourPun, IMouseTileSelectionCallback
{

    public int RestMovementInTurn => Hero.restMovementInTurn;
    public Hero Hero => Player.CurrentActiveHero;

    public MouseToHoveredMapTile mouseToHovoredMapTile;

    protected CallbackSet<IMouseTileSelectionCallback> MouseCallbackSet => mouseToHovoredMapTile.subscribers;

    protected List<Vector2Int> pathToCurrentHovoredTile;

    protected bool IsCurrentHovoredTileInRange => pathToCurrentHovoredTile.Count <= RestMovementInTurn;

    protected MapTile currentHoveredTile;

    protected bool isInAnimation;

    protected MarkerMapping mapMovementMarker;

    public void BeginTileHover(MapTile tile)
    {
        if (!GameManager.AllowPlayerMovement || !tile.CanBeEntered(Hero.CanEnterWater))
            return;

        currentHoveredTile = tile;
        DisplayPath(Hero.MapTile.Coordinates, tile.Coordinates);
    }

    public void ExitTileHovered(MapTile tile)
    {
        if (isInAnimation)
            return;
        currentHoveredTile = null;
        ClearPathDisplay();
    }

    protected void ClearPathDisplay()
    {
        if(mapMovementMarker == null)
        {
            Debug.LogWarning("Map markers are null. is that important?");
            return;
        }

        mapMovementMarker.ClearMarkers();
    }

    [PunRPC]
    public void MoveTowardsClickedTile(object[] pathParam, int heroIndex)
    {
        Vector2Int[] path = PhotonNetworkExtension.FromObjectArray<Vector2>(pathParam).ToVector2IntArray();
        Hero h = Heroes.GetHeroFromID(heroIndex);
        MapMovementAnimation moveAnim = MapAnimation.GetAnimationOfType<MapMovementAnimation>();
        moveAnim.AnimateMovement(path.ToList(), h, OnEndMovement);
    }

    protected void OnEndMovement()
    {
        isInAnimation = false;
        ExitTileHovered(currentHoveredTile);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && currentHoveredTile != null && IsCurrentHovoredTileInRange && GameManager.AllowPlayerMovement)
        {
            isInAnimation = true;
            object[] pathParam = PhotonNetworkExtension.ToObjectArray(VectorExtension.ToVector2Array(pathToCurrentHovoredTile));
            Broadcast.SafeRPC(
                photonView,
                nameof(MoveTowardsClickedTile),
                RpcTarget.All,
                () => MoveTowardsClickedTile(pathParam, Hero.heroIndex),
                pathParam, Hero.heroIndex);

        }
    }

    private void Start()
    {
        MouseCallbackSet.AddSubscriber(this);
    }

    private void OnDestroy()
    {
        ClearPathDisplay();
        MouseCallbackSet.RemoveSubscriber(this);
    }

    public void DisplayPath(Vector2Int start, Vector2Int end)
    {
        pathToCurrentHovoredTile = HexagonPathfinder.GetPath(start, end, false);
        mapMovementMarker = HexagonMarker.Instance.MarkHexagons(pathToCurrentHovoredTile, HexagonMarker.Instance.mapMovementMarker);

        int count = 1;
        foreach (GameObject marker in mapMovementMarker.markerMapping.Values)
        {
            MovementMapMarker m = marker.GetComponent<MovementMapMarker>();

            Color displayColor;
            if (count <= RestMovementInTurn)
            {
                displayColor = Color.green;
                m.distanceText.text = (RestMovementInTurn-count).ToString();
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

}
