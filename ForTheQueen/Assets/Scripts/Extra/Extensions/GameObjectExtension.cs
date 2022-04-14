using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{


    public static T GetOrAddComponent<T>(this Component c, Action<T> setupOnCreate = null) where T : Component
    {
        return c.gameObject.GetOrAddComponent(setupOnCreate);
    }

    public static T GetOrAddComponent<T>(this GameObject g, Action<T> setupOnCreate = null) where T : Component
    {
        T c = g.GetComponent<T>();
        if (c == null)
        {
            c = g.AddComponent<T>();
            setupOnCreate?.Invoke(c);
        }
        return c;
    }

}
