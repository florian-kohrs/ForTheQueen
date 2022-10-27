using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionEffect
{
    Blind, Slow, Bleed, Poison, Stun, SpeedUp, ArmorUp, ArmorDown, MagicArmorUp, MagicArmorDown, Taunt
}



public class ActionEffectInstance
{
    public int sufferedInRound;

    public int stackAmount;

    public static Color EffectToColor(ActionEffect e)
    {
        switch(e)
        {
            case ActionEffect.Blind:
                return Color.black;
                case ActionEffect.Slow:
                return Color.gray;
                case ActionEffect.Poison:
                return Color.green;
                case ActionEffect.Stun:
                return Color.yellow;
                case ActionEffect.SpeedUp:
                return (Color.white + Color.blue) / 2;
            case ActionEffect.MagicArmorDown:
            case ActionEffect.MagicArmorUp:
                return new Color(255, 192, 203, 1);
            case ActionEffect.ArmorDown:
            case ActionEffect.ArmorUp:
                return Color.blue;
            case ActionEffect.Bleed:
                return Color.red;
            case ActionEffect.Taunt:
                return new Color(160, 32, 240, 1);
            default:
                Debug.Log($"unhandled effect color {e}");
                return Color.white;
        }
    }
}
