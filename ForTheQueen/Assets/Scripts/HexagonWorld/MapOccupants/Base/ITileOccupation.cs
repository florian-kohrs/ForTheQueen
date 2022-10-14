using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileOccupation
{

    void OnPlayerMouseHover(Hero p);

    void OnPlayerMouseExit(Hero p);

    void OnPlayerUncovered(Hero p);

    void OnHeroEnter(Hero p, MapMovementAnimation mapMovement);

    void OnPlayerLeftFieldAfterStationary(Hero p);

    void OnPlayerReachedFieldAsTarget(Hero p);

    MapTile MapTile { get; set; }

    bool CanBeCrossed { get; }

    bool CanBeEntered { get; }

    void SpawnOccupation(Transform parent);

}
