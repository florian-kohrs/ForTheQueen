using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseTileSelectionCallback
{

    void BeginTileHover(MapTile tile);

    void ExitTileHovered(MapTile tile);

}
