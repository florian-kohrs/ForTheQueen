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
}
