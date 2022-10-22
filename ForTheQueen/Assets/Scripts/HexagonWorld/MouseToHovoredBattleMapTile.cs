using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToHovoredBattleMapTile : MonoBehaviour
{

    public BattleMap map;

    protected static int WORLD_LAYER_ID = 7;


    public MouseToHovoredMapTile<IBattleParticipant> mouseEvents;

    public CallbackSet<IMouseTileSelectionCallback<IBattleParticipant>> subscribers => mouseEvents.subscribers;


    private void Awake()
    {
        mouseEvents = new MouseToHovoredMapTile<IBattleParticipant>(map);
        mouseEvents.rayLayerId = WORLD_LAYER_ID;
    }
}
