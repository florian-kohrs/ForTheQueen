using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMapMovement : BaseMapMovement<Hero, MapTile>, IMouseTileSelectionCallback<MapTile>
{

    public MouseWorldEvents mouseToHovoredMapTile;

    public HexagonWorld world;

    protected CallbackSet<IMouseTileSelectionCallback<MapTile>> MouseCallbackSet => mouseToHovoredMapTile.subscribers;

    protected override Hero CurrentAgent => Player.CurrentActiveHero;

    protected override HexagonGrid<MapTile> Grid => world;

    public MapMovementAnimation anim;

    public override void RegisterMouseCallback()
    {
        MouseCallbackSet.AddSubscriber(this);
    }

    public override void UnregisterMouseCallback()
    {
        MouseCallbackSet.RemoveSubscriber(this);
    }

    public override void AnimateMovement(Vector2Int[] path)
    {
        anim.AnimateMovement(path.ToList(), CurrentAgent, OnEndMovement);
    }
}
