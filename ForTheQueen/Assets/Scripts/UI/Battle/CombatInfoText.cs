using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatInfoText : AdaptableInterfaceMask<string>
{

    public TextMeshProUGUI infoText;

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => false;

    public override bool BlockPlayerActiveAction => false;

    public override bool BlockPlayerPassiveAction => false;

    protected override void AdaptUITo(string value, Vector3 pos)
    {
        infoText.text = value;
    }
}
