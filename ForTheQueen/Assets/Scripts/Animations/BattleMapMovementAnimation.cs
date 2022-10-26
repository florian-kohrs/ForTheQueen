using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapMovementAnimation : MapAnimation
{


    public const float TIME_TO_NEXT_MAP_TILE = 0.25f;


    public BattleMap map;

    protected IBattleParticipant movingParticipant;

    protected List<Vector2Int> path;

    protected int currentPathIndex;

    public bool IsPathDone => currentPathIndex >= path.Count - 1;

    public void AnimateMovement(List<Vector2Int> path, IBattleParticipant h, Action onAnimationEnded = null)
    {
        if (path.Count == 0)
        {
            onAnimationEnded?.Invoke();
        }
        else
        {
            this.path = path;
            this.path.Insert(0, h.CurrentTile);
            movingParticipant = h;
            onAnimationDone = onAnimationEnded;
            currentPathIndex = 1;
            animationPlayer = MoveAlongPath();
            BeginAnimation();
            StartCoroutine(animationPlayer);
        }
    }

    protected IEnumerator MoveAlongPath()
    {
        BattleMapTile previousTile = map.DataFromIndex(movingParticipant.CurrentTile);
        BattleMapTile nextTile = null;
        for (; currentPathIndex < path.Count; currentPathIndex++)
        {
            nextTile = map.DataFromIndex(path[currentPathIndex]);
            yield return MoveToNextTile(nextTile);
            previousTile.participant = null;
            nextTile.participant = movingParticipant;
            movingParticipant.CurrentTile = nextTile.Coordinates;
            movingParticipant.RestMovementInTurn -= 1;
            previousTile = nextTile;
        }
        EndAnimation();
    }

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => true;

    public override bool BlockPlayerActiveAction => true;

    public override bool BlockPlayerPassiveAction => false;

    protected IEnumerator MoveToNextTile(BaseMapTile tile)
    {
        Transform heroTransform = movingParticipant.gameObject.transform;
        Vector3 startPos = heroTransform.position;
        Vector3 targetPos = tile.CenterPos;
        float currentTime = 0;
        heroTransform.LookAt(targetPos);
        while (currentTime < TIME_TO_NEXT_MAP_TILE)
        {
            currentTime += Time.deltaTime;
            Vector3 newPos = Vector3.Lerp(startPos, targetPos, currentTime / TIME_TO_NEXT_MAP_TILE);
            heroTransform.position = newPos;
            yield return null;
        }
        heroTransform.position = targetPos;
    }


}
