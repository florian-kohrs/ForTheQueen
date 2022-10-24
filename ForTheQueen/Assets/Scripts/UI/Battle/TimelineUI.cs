using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineUI : MonoBehaviour, IParticipantUIReference
{

    public Image imageReference;
    
    public Image ImageReference => imageReference;

    public HashSet<IParticipantUIReference> registeredInSet;

    public HashSet<IParticipantUIReference> RegisteredInSet { set => registeredInSet = value; }

    private void OnDestroy()
    {
        registeredInSet.Remove(this);
    }

}
