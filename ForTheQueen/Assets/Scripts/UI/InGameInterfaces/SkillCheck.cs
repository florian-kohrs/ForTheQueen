using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCheck
{

    public SkillCheck() { }

    public static SkillCheck GetMovementSkillCheckForHero(Hero h)
    {
        SkillCheck skillCheck = new SkillCheck();
        skillCheck.skill = Skill.Speed;
        skillCheck.stats = h.heroStats;
        TileBiom b = h.MapTile.kingdomOfMapTile.KingdomBiom;
        skillCheck.numberFocusUsed = b.guaranteedMovement;
        skillCheck.numberSkillChecks = b.guaranteedMovement + b.maxExtraMovement;
        return skillCheck;
    }

    public enum Skill { Strength, Agility, Speed, Intelligence, Talent, Vitality }

    public Skill skill;

    public int numberFocusUsed;

    public int numberSkillChecks;

    public CreatureStats stats;

}
