using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseWorldEvents : MonoBehaviour, IMouseTileSelectionCallback<MapTile>
{

    public MouseToHovoredMapTile<MapTile> mouseEvents;

    public HexagonWorld world;

    private void Awake()
    {
        HexagonWorld.instance.onWorldCreated.Add(OnWorldCreated);
        enabled = false;
    }


    protected void OnWorldCreated()
    {
        mouseEvents = new MouseToHovoredMapTile<MapTile>(world);
        mouseEvents.timeBeforeCallOnMouseStay = TIME_BEFORE_MOUSE_STAY_EVENT;
        mouseEvents.rayLayerId = WORLD_LAYER_ID;
        mouseEvents.subscribers.AddSubscriber(this);
        enabled = true;
    }


    protected const float TIME_BEFORE_MOUSE_STAY_EVENT = 0.5f;

    protected static int WORLD_LAYER_ID = 7;

    public CallbackSet<IMouseTileSelectionCallback<MapTile>> subscribers => mouseEvents.subscribers;

    protected MapTile lastHovoredTile => mouseEvents.LastHoveredTile;

    private void Update()
    {
        mouseEvents.Update();
    }

    public void BeginTileHover(Vector2Int coord, MapTile tile)
    {
    }

    public void OnMouseStay(Vector2Int coord, MapTile tile)
    {
        tile.OnMouseStay(Heroes.GetHeroWithActiveTurn());
    }

    public void ExitTileHovered(Vector2Int coord, MapTile tile)
    {
        tile.OnMouseExit(Heroes.GetHeroWithActiveTurn());
    }
}
