using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">the type which stores the data for this saveable monobehaviour</typeparam>
public abstract class SaveableMonoBehaviour : BaseSaveableMonoBehaviour
{

    [Save]
    private bool isEnabled;

    /// <summary>
    /// if the existence of a script should be saved, but no data behind it this can be set to "true".
    /// </summary>
    [Save]
    public bool dontSaveScriptValues;
    
    /// <summary>
    /// since the field "enabled" in the base class doesn`t have the "Save"-attribute
    /// the variable is kinda copied here with the "Save"-Attribute.
    /// The value is set from its GameObject when the scene is going to be saved
    /// </summary>
    public bool IsEnabled
    {
        get { return isEnabled; }
        set { isEnabled = value; }
    }
    
    protected virtual void setDataBeforeSaving(GamePersistence.SaveType saveType) { }
    
    public sealed override void setSaveData(GamePersistence.SaveType saveType)
    {
        IsEnabled = enabled;
        setDataBeforeSaving(saveType);
    }

    /// <summary>
    /// is called when the game was loaded, and the script is fully 
    /// initiated 
    /// </summary>
    public sealed override void onBehaviourLoaded()
    {
        enabled = isEnabled;
        BehaviourLoaded();
        OnAwake();
    }

    /// <summary>
    /// this method will only get called the first time an object is instantiated,
    /// and not when it was loaded later in game.
    /// </summary>
    protected virtual void OnFirstTimeBehaviourAwakend() { }

    /// <summary>
    /// will get called instead on "onAwake" when the game was loaded
    /// </summary>
    protected virtual void BehaviourLoaded() { }
 
    
    /// <summary>
    /// do not override this method by any mean with the "new" keyword, as Unity
    /// wont call this method anymore, which is essential for saving this script.
    /// Use the virutal function "OnAwake" instead!! Be aware tho that "OnAwake" is 
    /// not called, when the game is loaded. In this case the method "behaviourLoaded" will
    /// be called.
    /// </summary>
    protected void Awake()
    {
        ///only call virutal "onAwake" method when the object was not loaded.
        if (SaveableGame.FirstTimeSceneLoaded)
        {
            OnAwake();
            OnFirstTimeBehaviourAwakend();
        }
    }

}
