using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerellLookup : MonoBehaviour
{

    public static GenerellLookup instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public CustomizationLookup customizationLookup;

    public SpriteLookup spriteLookup;

}
