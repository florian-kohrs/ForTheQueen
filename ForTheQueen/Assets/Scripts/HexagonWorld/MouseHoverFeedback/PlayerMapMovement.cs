using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMapMovement : PunLocalBehaviour, IMouseTileSelectionCallback
{

    public HexagonWorld hexagonWorld;

    public int restMovementInTurn = 5;

    public GameObject markerPrefab;

    public Hero hero;
    
    public void BeginTileHover(MapTile tile)
    {
        DisplayPath(hero.MapTile.Coordinates, tile.Coordinates);
    }

    public void ExitTileHovered(MapTile tile)
    {
        hexagonWorld.marker.DeleteOldMarks();
    }

    private void OnEnable()
    {
        hexagonWorld.GetMouseCallbackSet.AddSubscriber(this);
    }

    private void OnDisable()
    {
        hexagonWorld.marker.DeleteOldMarks();
        hexagonWorld.GetMouseCallbackSet.RemoveSubscriber(this);
    }

    public void DisplayPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = hexagonWorld.pathfinder.GetPath(start, end);
        List<GameObject> markers = hexagonWorld.MarkHexagons(path.Skip(1), markerPrefab);

        int count = 1;
        foreach (GameObject marker in markers)
        {
            MovementMapMarker m = marker.GetComponent<MovementMapMarker>();

            Color displayColor;
            if (count <= restMovementInTurn)
            {
                displayColor = Color.green;
                m.distanceText.text = (restMovementInTurn-count).ToString();
            }
            else
            {
                m.distanceText.text = string.Empty;
                displayColor = Color.gray;
            }

            displayColor.a = 0.7f;
            m.hexagonOverlayRenderer.material.color = displayColor;

            count++;
        }
    }

    private void OnDestroy()
    {
        hexagonWorld.GetMouseCallbackSet.RemoveSubscriber(this);
    }

}
