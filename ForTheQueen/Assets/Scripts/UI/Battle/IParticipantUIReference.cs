using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IParticipantUIReference 
{
   
    Image ImageReference { get; }

    HashSet<IParticipantUIReference> RegisteredInSet { set; }

}
