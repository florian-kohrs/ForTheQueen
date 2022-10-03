using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AdaptableInterfaceMask<T> : InterfaceMask
{

    protected T adaptValue;

    public void AdaptUI(T value)
    {
        adaptValue = value;
    }

    protected abstract void AdaptUITo(T value);

}
