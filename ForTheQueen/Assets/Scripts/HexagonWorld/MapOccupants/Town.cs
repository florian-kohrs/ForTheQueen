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

    public string TownName => OccupationObject.occupationName;

    public override void OnPlayerEntered(Hero p)
    {
    }

    public override void OnPlayerReachedFieldAsTarget(Hero p)
    {
        InterfaceController.GetInterfaceMask<TownUI>().AdaptUI(this);
        p.OnEnterCity();
    }

    public override void OnPlayerLeftFieldAfterStationary(Hero p)
    {
        p.OnExitCity();
    }

    public override void OnPlayerUncovered(Hero p)
    {
    }

    [System.NonSerialized]
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


    public override void OnPlayerMouseHover(Hero p)
    {
        if (GameManager.AllowPlayerPassiveActions)
        {
            HoverInfo.AdaptUI(OccupationObject, MapTile.CenterPos);
            InterfaceController.Instance.AddMask(HoverInfo);
        }
    }

    public override void OnPlayerMouseExit(Hero p)
    {
        InterfaceController.Instance.RemoveMask(HoverInfo);
    }


}
