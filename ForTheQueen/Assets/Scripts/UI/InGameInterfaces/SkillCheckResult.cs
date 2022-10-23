using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckResult
{

    public SkillCheck skillCheck;

    public int numberSuccessfull;

    public bool WasPerfect => skillCheck.numberSkillChecks == numberSuccessfull;

    public bool CritFail => skillCheck.numberSkillChecks == 0;

    public float SucessRate => numberSuccessfull / (float)skillCheck.numberSkillChecks; 

}
