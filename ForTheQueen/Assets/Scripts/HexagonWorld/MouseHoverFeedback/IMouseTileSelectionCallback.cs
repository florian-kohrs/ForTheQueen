using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseTileSelectionCallback
{
    void EnterTileHovered(MapTile tile);

    void ExitTileHovered(MapTile tile);

}
