using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdaptableInterfaceMask<T> : InterfaceMask
{

    protected T adaptValue;

    public void AdaptUIAndOpen(T value, Vector3 pos = default)
    {
        AdaptUI(value, pos);
        interfaceController.AddMask(this);
    }

    public void AdaptUI(T value, Vector3 pos = default)
    {
        adaptValue = value;
        AdaptUITo(value,pos);
    }

    protected abstract void AdaptUITo(T value, Vector3 pos);

}
