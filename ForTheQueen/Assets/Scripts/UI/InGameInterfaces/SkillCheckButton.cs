using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckButton : MultiClickButton
{

    public SkillCheck skillCheck;

    protected Hero CurrentHero => Heroes.GetCurrentActiveHero();

    public Action leftClickAction;

    protected bool activated = false;

    public static Action currentSkillCheckAction;

    private void Start()
    {
        activated = false;
        leftClick.AddListener(()=> { activated = true; LeftClick(); });
        rightClick.AddListener(() => UseFocus());
        onBeginHover.AddListener(() => OnBeginnHover());
        onEndHover.AddListener(() => OnEndHover());
    }

    public void LeftClick()
    {
        if (activated || !Player.IsMyTurn)
            return;

        leftClickAction();
    }

    public void UseFocus()
    {
        if (activated || !Player.IsMyTurn)
            return;

        if (skillCheck.CanAddFocus) //add networking to button interactions
            PunBroadcastCommunication.UseFocusRPC();
    }

    public void OnBeginnHover()
    {
        if (activated || !Player.IsMyTurn)
            return;

        InterfaceController.GetInterfaceMask<SkillCheckUI>().AdaptUIAndOpen(skillCheck);
    }

    public void OnEndHover()
    {
        if (activated || !Player.IsMyTurn)
            return;

        InterfaceController.Instance.RemoveMask<SkillCheckUI>();
    }

}
