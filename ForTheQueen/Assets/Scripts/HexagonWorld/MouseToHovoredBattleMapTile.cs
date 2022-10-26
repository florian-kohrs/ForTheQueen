using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToHovoredBattleMapTile : MonoBehaviourPun
{

    public BattleMap map;

    protected static int WORLD_LAYER_ID = 7;


    public MouseToHovoredMapTile<BattleMapTile> mouseEvents;

    public CallbackSet<IMouseTileSelectionCallback<BattleMapTile>> subscribers => mouseEvents.subscribers;

    private void Awake()
    {
        mouseEvents = new MouseToHovoredMapTile<BattleMapTile>(map);
        mouseEvents.rayLayerId = WORLD_LAYER_ID;
    }

    private void Update()
    {
        mouseEvents.Update();
    }

}
