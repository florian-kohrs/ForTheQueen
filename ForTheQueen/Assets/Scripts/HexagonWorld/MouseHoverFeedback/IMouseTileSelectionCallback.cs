using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseTileSelectionCallback<T>
{

    void BeginTileHover(T tile);

    void OnMouseStay(T tile);

    void ExitTileHovered(T tile);

}
