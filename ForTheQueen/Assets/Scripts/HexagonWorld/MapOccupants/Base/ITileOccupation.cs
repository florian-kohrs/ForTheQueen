using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileOccupation
{

    void OnPlayerMouseHover(Hero p);

    void OnPlayerMouseExit(Hero p);

    void OnPlayerUncovered(Hero p);

    void OnPlayerEntered(Hero p);

    void OnPlayerLeftFieldAfterStationary(Hero p);

    void OnPlayerReachedFieldAsTarget(Hero p);

    MapTile MapTile { set; }

    bool CanBeCrossed { get; }

    bool CanBeEntered { get; }

    void SpawnOccupation(Transform parent);

}
