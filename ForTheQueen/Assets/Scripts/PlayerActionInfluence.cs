using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActionInfluence : MonoBehaviour
{

    public abstract bool BlockCameraMovement {get;}

    public abstract bool BlockPlayerMovement { get; }

    public abstract bool BlockPlayerActiveAction { get; }

    public abstract bool BlockPlayerPassiveAction { get; }

    protected void ApplyInfluence()
    {
        if (BlockCameraMovement)
            GameManager.blockCameraMovement.Add(this);

        if (BlockPlayerMovement)
            GameManager.blockPlayerMovement.Add(this);

        if (BlockPlayerActiveAction)
            GameManager.blockPlayerActiveAction.Add(this);

        if (BlockPlayerPassiveAction)
            GameManager.blockPlayerPassiveAction.Add(this);
    }

    protected void RemoveInfluence()
    {
        if (BlockCameraMovement)
            GameManager.blockCameraMovement.Remove(this);

        if (BlockPlayerMovement)
            GameManager.blockPlayerMovement.Remove(this);

        if (BlockPlayerActiveAction)
            GameManager.blockPlayerActiveAction.Remove(this);

        if (BlockPlayerPassiveAction)
            GameManager.blockPlayerPassiveAction.Remove(this);
    }

    protected void OnDestroy()
    {
        RemoveInfluence();
        BeforeDestroy();
    }

    protected virtual void BeforeDestroy() { }

}
