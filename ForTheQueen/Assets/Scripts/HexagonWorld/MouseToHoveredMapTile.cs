using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseToHoveredMapTile : MonoBehaviour
{

    protected const float TIME_BEFORE_MOUSE_STAY_EVENT = 0.5f;

    protected static int WORLD_LAYER_ID = 7;

    public CallbackSet<IMouseTileSelectionCallback> subscribers = new CallbackSet<IMouseTileSelectionCallback>();

    protected MapTile lastHovoredTile;

    protected float timeOnHoveredTile;

    protected bool calledTileHover;


    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000/*, WORLD_LAYER_ID*/))
        {
            Maybe<MapTile> tile = HexagonWorld.GetWorldTileFromPosition(hit.point);

            if (tile.HasValue)
            {
                //hexagonWorld.MarkHexagons(hexagonWorld.GetAdjencentTiles(tile.Value));
            }

            NotifySubscriberOnChange(tile);
        }
    }

    protected void ResetTileMouseHover()
    {
        if (calledTileHover)
            lastHovoredTile.OnMouseExit(null);
        timeOnHoveredTile = 0;
        calledTileHover = false;
    }

    protected void NotifySubscriberOnChange(Maybe<MapTile> tile)
    {
        ///increase tile mouse hover time if old and new hover is the same
        if (tile.Value == lastHovoredTile)
        {
            timeOnHoveredTile += Time.deltaTime;
            if (tile.HasValue && timeOnHoveredTile > TIME_BEFORE_MOUSE_STAY_EVENT && !calledTileHover)
            {
                calledTileHover = true;
                lastHovoredTile.OnMouseStay(Player.CurrentActiveHero);
            }
        }
        else 
        { 
            ResetTileMouseHover();

            if (lastHovoredTile != null)
            {
                subscribers.CallForEachSubscriber(s => s.ExitTileHovered(lastHovoredTile));
            }

            if (tile.HasValue)
            {
                subscribers.CallForEachSubscriber(s => s.BeginTileHover(tile.Value));
            }
            lastHovoredTile = tile.Value;
        }
    }

}
