using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseToHoveredMapTile : MonoBehaviour
{

    protected static int WORLD_LAYER_ID = 7;

    public HexagonWorld hexagonWorld;

    public HexagonMarker marker;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2000/*, WORLD_LAYER_ID*/))
        {
            Maybe<WorldTile> tile = hexagonWorld.GetWorldTileFromPosition(hit.point);
            if(tile.HasValue)
            {
                marker.MarkHexagons(hexagonWorld.GetAdjencentTiles(tile.Value));
            }

            //calculate array position and find actual hit one by local search and check each tile if mouse pointer is inside
        }
    }

}
