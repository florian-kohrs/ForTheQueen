using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Town : TileInteractableOccupation<TownScriptableObject>
{

    public Town() { }

    public Town(TownScriptableObject town) : base(town)
    { }

    public Inventory itemsToSell;

    public string TownName => occupationObject.occupationName;

    public override void OnPlayerEntered()
    {
    }

    public override void OnPlayerReachedFieldAsTarget()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerUncovered()
    {
    }

    protected GenericMouseHoverInfo hoverInfo;

    protected GenericMouseHoverInfo HoverInfo
    {
        get
        {
            if(hoverInfo == null)
                hoverInfo = InterfaceController.GetInterfaceMask<GenericMouseHoverInfo>();
            return hoverInfo;
        }
    }


    public override void OnPlayerMouseHover()
    {
        HoverInfo.AdaptUI(OccupationObject, MapTile.CenterPos);
        InterfaceController.Instance.AddMask(HoverInfo);
    }

    public override void OnPlayerMouseExit()
    {
        InterfaceController.Instance.RemoveMask(HoverInfo);
    }


}
