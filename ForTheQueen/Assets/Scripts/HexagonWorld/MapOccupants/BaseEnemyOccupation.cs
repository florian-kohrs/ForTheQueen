using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEnemyOccupation : TileInteractableOccupation<BaseEnemyOccupationScriptableObject>, IBaseEnemyOccupation
{

    protected float DespawnTime => 0.4f * GameManager.AnimationSpeed;

    public void Despawn()
    {
        Delete();
    }

    public void Delete()
    {
        MapTile.RemoveTileOccupation(this);
        GameObject.Destroy(mapOccupationInstance);
    }


    public override void OnPlayerEntered(Hero p)
    {
        //TODO: Open Fight UI
    }

    public override void OnPlayerMouseExit(Hero p)
    {
        InterfaceController.Instance.RemoveMask<GenericMouseHoverInfo>();
    }

    public override void OnPlayerMouseHover(Hero p)
    {
        InterfaceController.GetInterfaceMask<GenericMouseHoverInfo>().AdaptUIAndOpen(OccupationObject, mapTile.CenterPos);
    }

    public override void OnPlayerReachedFieldAsTarget(Hero p)
    {
    }

    public override void OnPlayerUncovered(Hero p)
    {
    }

}
