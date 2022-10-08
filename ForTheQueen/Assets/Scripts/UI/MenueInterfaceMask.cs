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

    protected MenueInterfaceMask openedAdditionaly;

    public void ChangeInterfaceFor(MenueInterfaceMask mask)
    {
        interfaceController.RemoveMask(this);
        mask.openedByMenue = this;
        interfaceController.AddMask(mask);
    }

    public void OpenAdditiveInterface(MenueInterfaceMask mask)
    {
        if(openedAdditionaly != null)
            interfaceController.RemoveMask(openedAdditionaly);

        interfaceController.AddMask(mask);
        openedAdditionaly = mask;
        mask.openedByMenue = this;
    }

    public void NavigateBack()
    {
        ChangeInterfaceFor(openedByMenue);
    }

}
