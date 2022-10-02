using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileOccupation
{

    void OnPlayerMouseHover();

    bool CanBeCrossed { get; }

    bool CanBeEntered { get; }

}
