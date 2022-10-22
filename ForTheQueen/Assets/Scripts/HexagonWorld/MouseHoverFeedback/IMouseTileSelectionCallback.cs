using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseTileSelectionCallback<T>
{

    void BeginTileHover(Vector2Int coord, T tile);

    void OnMouseStay(Vector2Int coord, T tile);

    void ExitTileHovered(Vector2Int coord, T tile);

}
