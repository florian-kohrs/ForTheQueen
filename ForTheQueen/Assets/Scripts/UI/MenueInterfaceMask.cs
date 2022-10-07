using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenueInterfaceMask : InterfaceMask
{

    protected void SetEnabledOfButton(Button b, bool enabled)
    {
        b.interactable = enabled;
    }

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => false;

    public override bool BlockPlayerActiveAction => false;

    public override bool BlockPlayerPassiveAction => false;

    protected MenueInterfaceMask openedByMenue;

    public void ChangeInterfaceFor(MenueInterfaceMask mask)
    {
        interfaceController.RemoveMask(this);
        mask.openedByMenue = this;
        interfaceController.AddMask(mask);
    }

    public void NavigateBack()
    {
        ChangeInterfaceFor(openedByMenue);
    }

}
