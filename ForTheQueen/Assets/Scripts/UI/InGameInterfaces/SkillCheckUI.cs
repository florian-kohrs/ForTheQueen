using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckUI : AdaptableInterfaceMask<SkillCheck>
{

    public GameObject skillCheckPrefab;

    public Transform skillCheckParent;

    protected List<Image> skillCheckInstances;

    protected SkillCheck currentSkillCheck;

    protected Action<SkillCheckResult> onEvaluationDone;

    public static readonly Color FOCUS_COLOR = (Color.yellow + Color.red) / 2;
    public static readonly Color UNCLEAR_COLOR = new Color(.5f,.5f,.5f,.5f);
    public static readonly Color SUCCESS_COLOR = Color.green;
    public static readonly Color FAIL_COLOR = Color.red;

    public const float SKILL_ANIMATION_SPEED = 0.4f;

    public const float IDLE_AFTER_ANIMATION = 0.25f;

    public override bool BlockCameraMovement => false;

    public override bool BlockPlayerMovement => true;

    public override bool BlockPlayerActiveAction => true;

    public override bool BlockPlayerPassiveAction => false;

    protected override void AdaptUITo(SkillCheck value, Vector3 pos)
    {
        currentSkillCheck = value;
        skillCheckInstances = new List<Image>(value.numberSkillChecks);
        for (int i = 0; i < value.numberSkillChecks; i++)
        {
            SpawnMarker(value.numberFocusUsed > i);
        }
    }

    protected void SpawnMarker(bool usedFocus)
    {
        GameObject skillCheckMarker = Instantiate(skillCheckPrefab, skillCheckParent);
        Color c = usedFocus ? FOCUS_COLOR : UNCLEAR_COLOR;
        Image img = skillCheckMarker.GetComponent<Image>();
        img.color = c;
        skillCheckInstances.Add(img);
        skillCheckMarker.transform.GetChild(0).GetComponent<Image>().sprite = GenerellLookup.instance.spriteLookup.SpriteFromSkillCheck(currentSkillCheck.skill);
    }

    public void EvaluateSkillCheck(Action<SkillCheckResult> onEvaluationDone)
    {
        this.onEvaluationDone = onEvaluationDone;
        StartSkillCheck();
    }

    protected override void OnClose()
    {
        foreach (var item in skillCheckInstances)
        {
            Destroy(item);
        }
    }

    protected void StartSkillCheck()
    {
        IEnumerator animation = SkillCheckAnimation();
        StartCoroutine(animation);
    }



    protected IEnumerator SkillCheckAnimation()
    {
        System.Random rand = GameInstanceData.Rand;
        SkillCheckResult result = new SkillCheckResult();
        result.skillCheck = currentSkillCheck;
        result.numberSuccessfull = currentSkillCheck.numberFocusUsed;
        for (int i = currentSkillCheck.numberFocusUsed; i < currentSkillCheck.numberSkillChecks; i++)
        {
            yield return new WaitForSeconds(SKILL_ANIMATION_SPEED);
            int r = rand.Next(0, 100);
            bool success = r < currentSkillCheck.stats.GetStatsOfSkill(currentSkillCheck.skill);
            if(success)
            {
                skillCheckInstances[i].color = SUCCESS_COLOR;
                result.numberSuccessfull++;
            }
            else
            {
                skillCheckInstances[i].color = FAIL_COLOR;
            }
        }
        yield return new WaitForSeconds(IDLE_AFTER_ANIMATION);
        RemoveMask();
        onEvaluationDone(result);
    }

}
