using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackSet<T>
{

    public HashSet<T> subscribers = new HashSet<T>();

    public void AddSubscriber(T subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void RemoveSubscriber(T subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void CallForEachSubscriber(Action<T> a)
    {
        foreach (var subscriber in subscribers)
            a(subscriber);
    }

}
