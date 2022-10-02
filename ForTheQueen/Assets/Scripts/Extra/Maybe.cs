using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maybe<T> /*where T : struct*/
{

    public Maybe() { }

    public Maybe(T value) { Value = value; }   

    private T value;

    protected bool hasValue;

    public void RemoveValue()
    {
        value = default;
        hasValue = false;
    }

    public T Value
    {
        get 
        {   
            return value; 
        }
        set 
        { 
            hasValue = true;
            this.value = value; 
        }
    }

    public Maybe<K> ApplyValueToFunction<K>(System.Func<T,K> f)
    {
        if (HasValue)
            return new Maybe<K>(f(Value));
        else
            return new Maybe<K>();
    }

    public bool HasValue => hasValue;


}
