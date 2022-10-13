using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckResult
{

    public SkillCheck skillCheck;

    public int numberSuccessfull;

    public bool WasPerfect => skillCheck.numberSkillChecks == numberSuccessfull;

}
