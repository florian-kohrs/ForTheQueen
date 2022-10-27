using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToHovoredMapTile<T> where T : BaseMapTile
{

    public MouseToHovoredMapTile(HexagonGrid<T> grid)
    {
        this.grid = grid;
    }

    public float timeBeforeCallOnMouseStay = 0.5f;

    public HexagonGrid<T> grid;

    protected T lastHoveredTile;

    public T LastHoveredTile => lastHoveredTile;

    protected Vector2Int lastHoveredTileCoord;

    public CallbackSet<IMouseTileSelectionCallback<T>> subscribers = new CallbackSet<IMouseTileSelectionCallback<T>>();

    protected float timeOnHoveredTile;

    protected bool calledTileHover;

    public int rayLayerId = 7;

    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000/*, WORLD_LAYER_ID*/))
        {
            Maybe<Vector2Int> coord = grid.GetIndexFromPosition(hit.point);
            Maybe<T> tile = coord.ApplyValueToFunction(grid.DataFromIndex);

            if (tile.HasValue)
            {
                //hexagonWorld.MarkHexagons(hexagonWorld.GetAdjencentTiles(tile.Value));
            }

            NotifySubscriberOnChange(coord, tile);
        }
    }

    protected void NotifySubscriberOnChange(Maybe<Vector2Int> coord, Maybe<T> tile)
    {
        ///increase tile mouse hover time if old and new hover is the same
        if (coord.HasValue && Equals(coord.Value, lastHoveredTileCoord))
        {
            timeOnHoveredTile += Time.deltaTime;
            if (tile.HasValue && timeOnHoveredTile > timeBeforeCallOnMouseStay && !calledTileHover)
            {
                calledTileHover = true;
                subscribers.CallForEachSubscriber(s => s.OnMouseStay(lastHoveredTile));
            }
        }
        else
        {
            ResetTileMouseHover();

            if (lastHoveredTile != null)
            {
                subscribers.CallForEachSubscriber(s => s.ExitTileHovered(lastHoveredTile));
            }

            if (tile.HasValue)
            {
                subscribers.CallForEachSubscriber(s => s.BeginTileHover(tile.Value));
            }
            lastHoveredTile = tile.Value;
            if(coord.HasValue)
                lastHoveredTileCoord = coord.Value;
        }
    }

    protected void ResetTileMouseHover()
    {
        timeOnHoveredTile = 0;
        lastHoveredTileCoord = new Vector2Int(int.MinValue, int.MinValue);  
        calledTileHover = false;
    }

}
