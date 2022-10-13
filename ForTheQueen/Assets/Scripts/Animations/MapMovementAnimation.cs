using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementAnimation : MapAnimation
{

    public const float TIME_TO_NEXT_MAP_TILE = 0.25f;

    protected Hero movingHero;

    protected List<Vector2Int> path;

    protected int currentPathIndex;

    public bool IsPathDone => currentPathIndex >= path.Count - 1; 

    public void AnimateMovement(List<Vector2Int> path, Hero h, Action onAnimationEnded = null)
    {
        if (path.Count == 0)
        {
            h.MapTile.OnPlayerReachedFieldAsTarget(h);
            onAnimationEnded?.Invoke();
        }
        else
        {
            this.path = path;
            this.path.Insert(0, h.MapTile.Coordinates);
            movingHero = h;
            onAnimationDone = onAnimationEnded;
            ContinuePath();
        }
    }

    protected IEnumerator MoveAlongPath()
    {
        movingHero.MapTile.OnPlayerLeftAfterStationary(movingHero);
        MapTile previousTile = movingHero.MapTile;
        MapTile nextTile = null;
        for (currentPathIndex = 1; currentPathIndex < path.Count && !movingHero.interuptMovement; currentPathIndex++)
        {
            nextTile = HexagonWorld.MapTileFromIndex(path[currentPathIndex]);
            yield return MoveToNextTile(nextTile);
            nextTile.OnPlayerEntered(movingHero, this);
            movingHero.restMovementInTurn -= 1;
        }
        if(currentPathIndex == path.Count)
        {
            nextTile.OnPlayerReachedFieldAsTarget(movingHero);
        }
        nextTile.AddTileOccupation(movingHero);
        previousTile.RemoveTileOccupation(movingHero);
        movingHero.interuptMovement = false;
        EndAnimation();
    }

    public void Backstep()
    {
        path = new List<Vector2Int>() { path[currentPathIndex - 1] };
        MoveAlongPath();
    }

    public void ContinuePath()
    {
        animationPlayer = MoveAlongPath();
        BeginAnimation();
        StartCoroutine(animationPlayer);
    }

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => true;

    public override bool BlockPlayerActiveAction => true;

    public override bool BlockPlayerPassiveAction => false;

    protected IEnumerator MoveToNextTile(MapTile tile)
    {
        Transform heroTransform = movingHero.runtimeHeroObject;
        Vector3 startPos = heroTransform.position;
        Vector3 targetPos = tile.CenterPos;
        float currentTime = 0;
        heroTransform.LookAt(targetPos);
        while (currentTime < TIME_TO_NEXT_MAP_TILE)
        {
            currentTime += Time.deltaTime;
            Vector3 newPos = Vector3.Lerp(startPos,targetPos, currentTime / TIME_TO_NEXT_MAP_TILE);
            heroTransform.position = newPos;
            yield return null;
        }
        heroTransform.position = targetPos;
    }

}
