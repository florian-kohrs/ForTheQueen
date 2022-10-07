using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListLookUp : MonoBehaviour
{

    public static ListLookUp instance;

    private void Awake()
    {
        instance = this;
    }

    public DesignerList classes;

    public DesignerList races;

}
