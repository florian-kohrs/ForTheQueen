using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillCheck
{

    public SkillCheck(Hero h) : this(h.heroStats)
    {
        hero = h;
    }

    public SkillCheck(CreatureStats c)
    {
        stats = c;
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

    public CreatureStats stats;

    public bool CanAddFocus => hero != null && canFocus && numberFocusUsed < numberSkillChecks && hero.CanSpendFocus;

    public CreatureStats Stats => stats;

    public int StandartSuccessPercentage => stats.GetStatsOfSkill(skill);

    public int PerfectRate => Mathf.RoundToInt(100 * (Mathf.Pow(StandartSuccessPercentage / 100f, numberSkillChecks)));

}
