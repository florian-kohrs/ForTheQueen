using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementBattleInteruption
{

    public MapMovementBattleInteruption(BattleParticipants participants, MapMovementAnimation mapMovement)
    {
        this.participants = participants;
        this.mapMovement = mapMovement;
    }

    public BattleParticipants participants;

    public MapMovementAnimation mapMovement;

}
