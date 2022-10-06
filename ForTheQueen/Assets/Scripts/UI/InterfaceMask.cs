using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterfaceMask : PlayerActionInfluence, IInterfaceMask
{

    public GameObject maskRoot;

    protected InterfaceController interfaceController;

    protected virtual void OnOpen() { }

    protected bool IsOpen => maskRoot.activeSelf;

    public void Open()
    {
        ApplyInfluence();
        maskRoot.SetActive(true);
        OnOpen();
    }

    protected void Start()
    {
        maskRoot.SetActive(false);
        interfaceController = InterfaceController.Instance;
        interfaceController.AddPossibleInterfaceMask(this);
    }

    public virtual CursorLockMode CursorMode => CursorLockMode.Confined;
    
    public int layer;

    public virtual bool BlockOtherInterfaces => false;
    
    public virtual bool DominantMask => false;

    protected override void BeforeDestroy()
    {
        interfaceController.RemovePossibleInterfaceMask(this);
    }

    protected virtual void OnClose() { }

    public void Close()
    {
        RemoveInfluence();
        maskRoot.SetActive(false);
        OnClose();
    }

}
