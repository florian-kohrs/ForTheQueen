using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteLookup
{

    public Sprite intelligenceSprite;
    public Sprite agilitySprite;
    public Sprite strengthSprite;
    public Sprite talentSprite;
    public Sprite speedSprite;
    public Sprite vitalitySprite;

    public Sprite SpriteFromSkillCheck(SkillCheck.Skill skill)
    {
        switch (skill)
        {
            case SkillCheck.Skill.Speed:
                return speedSprite;
            case SkillCheck.Skill.Strength:
                return strengthSprite;
            case SkillCheck.Skill.Talent:
                return talentSprite;
            case SkillCheck.Skill.Agility:
                return agilitySprite;
            case SkillCheck.Skill.Vitality:
                return vitalitySprite;
            case SkillCheck.Skill.Intelligence:
                return intelligenceSprite;
            default:
                throw new System.Exception($"No sprite available for skill{skill}");
        }
    }

}
