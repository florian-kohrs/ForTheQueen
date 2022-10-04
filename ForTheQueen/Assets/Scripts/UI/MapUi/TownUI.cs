using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownUI : AdaptableInterfaceMask<Town>
{

    public Image townImage;

    public TextMeshProUGUI townName;

    public void LeaveTown()
    {
        interfaceController.RemoveMask(this);
    }

    public void OpenMarket()
    {

    }

    public void OpenQuestBoard()
    {

    }

    public void OpenServices()
    {

    }

    protected override void AdaptUITo(Town value, Vector3 pos)
    {
        townImage.sprite = value.OccupationObject.townSprite;
        townName.text = value.TownName;
        interfaceController.AddMask(this);
    }
}
