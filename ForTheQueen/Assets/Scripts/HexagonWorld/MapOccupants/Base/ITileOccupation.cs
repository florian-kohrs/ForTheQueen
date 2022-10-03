using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileOccupation
{

    void OnPlayerMouseHover();

    void OnPlayerMouseExit();

    void OnPlayerUncovered();

    void OnPlayerEntered();

    void OnPlayerReachedFieldAsTarget();

    MapTile MapTile { set; }

    bool CanBeCrossed { get; }

    bool CanBeEntered { get; }

    void SpawnOccupation(Transform parent);

}
