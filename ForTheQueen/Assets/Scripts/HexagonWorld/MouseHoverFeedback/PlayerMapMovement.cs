using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMapMovement : MonoBehaviour, IMouseTileSelectionCallback
{

    public HexagonWorld hexagonWorld;

    public int restMovementInTurn = 5;

    public GameObject markerPrefab;
    
    public void EnterTileHovered(MapTile tile)
    {
        DisplayPath(new Vector2Int(0, 0), tile.Coordinates);
    }

    public void ExitTileHovered(MapTile tile)
    {
        hexagonWorld.marker.DeleteOldMarks();
    }

    private void Start()
    {
        hexagonWorld.GetMouseCallbackSet.AddSubscriber(this);
    }

    private void Update()
    {
        
    }

    public void DisplayPath(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = hexagonWorld.pathfinder.GetPath(start, end);
        List<GameObject> markers = hexagonWorld.MarkHexagons(path, markerPrefab);

        int count = 1;
        foreach (GameObject marker in markers)
        {
            MovementMapMarker m = marker.GetComponent<MovementMapMarker>();
            m.distanceText.text = count.ToString();

            Color displayColor;
            if (count < restMovementInTurn)
                displayColor = Color.green;
            else
                displayColor = Color.gray;

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
