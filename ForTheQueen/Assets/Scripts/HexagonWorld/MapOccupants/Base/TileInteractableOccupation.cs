using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileInteractableOccupation<T, Self, HoverUI> : TileOccupation<T> 
    where HoverUI : AdaptableInterfaceMask<Self> 
    where Self : TileInteractableOccupation<T, Self, HoverUI>
    where T : TileOccupationInteractableScritableObject
{

    public TileInteractableOccupation() { }

    public TileInteractableOccupation(T t) : base(t)
    { }

    public override bool CanBeCrossed => true;

    public override bool CanBeEntered => true;

    protected HoverUI ui;

    protected Self GetSelf => (Self)this;

    protected HoverUI Ui
    {
        get
        {
            if(ui == null)
                ui = InterfaceController.GetInterfaceMask<HoverUI>();
            return ui;
        }
    }

    public override void OnPlayerMouseHover()
    {
        Ui.AdaptUI(GetSelf);
        InterfaceController.AddMask(Ui);
    }

    public override void OnPlayerMouseExit()
    {
        InterfaceController.RemoveMask(Ui);
    }

}
