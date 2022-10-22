using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCheck
{

    public SkillCheck(Hero h) 
    {
        hero = h;
    }

    public static SkillCheck GetMovementSkillCheckForHero(Hero h)
    {
        SkillCheck skillCheck = new SkillCheck(h);
        skillCheck.skill = Skill.Speed;
        TileBiom b = h.MapTile.kingdomOfMapTile.KingdomBiom;
        skillCheck.numberFocusUsed = b.guaranteedMovement;
        skillCheck.numberSkillChecks = b.guaranteedMovement + b.maxExtraMovement;
        return skillCheck;
    }

    public enum Skill { Strength, Agility, Speed, Intelligence, Talent, Vitality }

    public Skill skill;

    public int numberFocusUsed;

    public int numberSkillChecks;

    public bool canFocus;

    public Hero hero;

    public bool CanAddFocus => canFocus && numberFocusUsed < numberSkillChecks && hero.CanSpendFocus;

    public CreatureStats Stats => hero.heroStats;

}
