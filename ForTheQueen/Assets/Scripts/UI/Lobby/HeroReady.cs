using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroReady : MonoBehaviourPun
{

    public HeroDesigner designer;

    public TextMeshProUGUI btnText;

    protected string readyText = "Ready";
    protected string notReadyText = "Not Ready";

    protected bool ready = false;

    protected int CurrentHeroId => designer.Hero.heroIndex;

    protected bool CurrentHeroReadyState => designer.Hero.customization.customizationDone;

    public void SwapHeroReadyState()
    {
        designer.SetButtonEnabledState(ready);
        Broadcast.SafeRPC(
            photonView, 
            nameof(SetHeroReadyState), 
            RpcTarget.All, 
            () => SetHeroReadyState(CurrentHeroId, !CurrentHeroReadyState), 
            CurrentHeroId, !CurrentHeroReadyState);
    }

    [PunRPC]
    public void SetHeroReadyState(int heroId, bool state)
    {
        ready = state;
        Heroes.GetHeroFromID(heroId).customization.customizationDone = ready;
        if (ready)
            btnText.text = readyText;
        else
            btnText.text = notReadyText;
    }

}
