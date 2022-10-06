using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MapAnimation : PlayerActionInfluence
{

    private void Awake()
    {
        animations.Add(this);
    }

    protected static HashSet<MapAnimation> animations = new HashSet<MapAnimation>();

    public static T GetAnimationOfType<T>() where T : MapAnimation
    {
        Type type = typeof(T);
        return (T)animations.Where(a => a.GetType().IsEquivalentTo(type)).First();
    } 

    public virtual bool BlockPlayerInputs => true;

    public IEnumerator animationPlayer;

    public Action onAnimationDone;

    protected void BeginAnimation()
    {
        ApplyInfluence();
    }

    protected void EndAnimation()
    {
        RemoveInfluence();
        onAnimationDone?.Invoke();
    }

    protected override void BeforeDestroy()
    {
        animations.Remove(this);
    }

}
