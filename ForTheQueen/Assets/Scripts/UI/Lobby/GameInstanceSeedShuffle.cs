using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstanceSeedShuffle : PunMasterBehaviour
{

    protected override void OnNotMaster()
    {
        
    }

    protected override void OnStart()
    {
        GameInstanceData.CurrentGameInstanceData.ShuffleGameInstanceSeed();
    }

}
