using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseToHoveredMapTile : MonoBehaviour
{

    protected static int WORLD_LAYER_ID = 7;

    public HexagonWorld hexagonWorld;

    public CallbackSet<IMouseTileSelectionCallback> subscribers = new CallbackSet<IMouseTileSelectionCallback>();

    protected MapTile lastHovoredTile;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000/*, WORLD_LAYER_ID*/))
        {
            Maybe<MapTile> tile = hexagonWorld.GetWorldTileFromPosition(hit.point);

            if (tile.HasValue)
            {
                //hexagonWorld.MarkHexagons(hexagonWorld.GetAdjencentTiles(tile.Value));
            }

            NotifySubscriberOnChange(tile);


        }
    }

    protected void NotifySubscriberOnChange(Maybe<MapTile> tile)
    {
        ///do nothing if the old and new tile hover are the same
        if (tile.Value == lastHovoredTile)
            return;

        if (lastHovoredTile != null)
        {
            subscribers.CallForEachSubscriber(s => s.ExitTileHovered(lastHovoredTile));
        }

        if (tile.HasValue)
        {
            subscribers.CallForEachSubscriber(s => s.EnterTileHovered(tile.Value));
        }
        lastHovoredTile = tile.Value;
    }

}
