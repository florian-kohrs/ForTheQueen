using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMount : IMovementController, IGetMovementController, IMovementPredicter
{

    bool CanMount(IRider r);

    ISaddle Saddle { get; set; }

    IRider MountedBy { get; }

    bool MountWith(IRider r);

    void Demount(IRider r);

    void OnRiderDemounted(IRider r);


}
