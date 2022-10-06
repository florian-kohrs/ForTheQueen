using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovementAnimation : MapAnimation
{

    public const float TIME_TO_NEXT_MAP_TILE = 0.25f;

    protected Hero movingHero;

    protected HexagonWorld world;

    protected List<Vector2Int> path;


    public void AnimateMovement(List<Vector2Int> path, Hero h, HexagonWorld world, Action onAnimationEnded = null)
    {
        if (path.Count == 0)
        {
            h.MapTile.OnPlayerReachedFieldAsTarget(h);
        }
        else
        {
            this.path = path;
            movingHero = h;
            this.world = world;
            onAnimationDone = onAnimationEnded;
            animationPlayer = MoveAlongPath();
            BeginAnimation();
            StartCoroutine(animationPlayer);
        }
    }

    protected IEnumerator MoveAlongPath()
    {
        movingHero.MapTile.OnPlayerLeftAfterStationary(movingHero);
        MapTile previousTile = movingHero.MapTile;
        MapTile nextTile = null;
        int i;
        for (i = 0; i < path.Count && !movingHero.interuptMovement; i++)
        {
            nextTile = world.MapTileFromIndex(path[i]);
            yield return MoveToNextTile(nextTile);
            nextTile.OnPlayerEntered(movingHero);
            movingHero.restMovementInTurn -= 1;
        }
        if(i == path.Count)
        {
            nextTile.OnPlayerReachedFieldAsTarget(movingHero);
        }
        nextTile.AddTileOccupation(movingHero);
        previousTile.RemoveTileOccupation(movingHero);
        movingHero.interuptMovement = false;
        EndAnimation();
    }

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => movingHero.IsMine;

    public override bool BlockPlayerActiveAction => movingHero.IsMine;

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
