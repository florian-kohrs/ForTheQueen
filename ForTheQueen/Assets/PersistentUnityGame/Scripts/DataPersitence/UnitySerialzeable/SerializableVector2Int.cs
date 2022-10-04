using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


[Serializable]
public class SerializableVector2Int : ISerializable
{

    public override bool Equals(object obj)
    {
        if (obj != null && obj is Serializable2DVector)
        {
            return v.Equals(((Serializable2DVector)obj).v);
        }
        else
        {
            return v.Equals(obj);
        }
    }

    public override int GetHashCode()
    {
        return v.GetHashCode();
    }

    public int x => v.x;

    public int y => v.y;

    private SerializableVector2Int(SerializationInfo info, StreamingContext context)
    {
        v.x = info.GetInt32("x");
        v.y = info.GetInt32("y");
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", v.x);
        info.AddValue("y", v.y);
    }

    public Vector2Int v;

    public SerializableVector2Int(int x, int y) : this(new Vector2Int(x, y)) { }

    public SerializableVector2Int(Vector2Int vector)
    {
        v = vector;
    }

    public static implicit operator Vector2Int(SerializableVector2Int vec)
    {
        return vec.v;
    }

    public static implicit operator SerializableVector2Int(Vector2Int vec)
    {
        return new SerializableVector2Int(vec);
    }

}

