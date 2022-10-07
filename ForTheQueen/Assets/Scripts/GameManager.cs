using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{

    static GameManager()
    {
        SceneManager.activeSceneChanged += delegate
        {
            instance = new GameManager();
        };
    }

    private GameManager()
    {
        Time.timeScale = 1;
    }

    private bool isGameFrozen;


    public static HashSet<object> blockCameraMovement = new HashSet<object>();

    public static HashSet<object> blockPlayerMovement = new HashSet<object>();

    public static HashSet<object> blockPlayerActiveAction = new HashSet<object>();

    public static HashSet<object> blockPlayerPassiveAction = new HashSet<object>();

    private InterfaceController interfaceController;

    public static bool IsMyTurn => Player.IsMyTurn;

    private InterfaceController InterfaceController
    {
        get
        {
            if (interfaceController == null)
            {
                interfaceController = InterfaceController.Instance;
            }
            return interfaceController;
        }
        set
        {
            interfaceController = value;
        }
    }

    public static void SetInterfaceController(InterfaceController interfaceController)
    {
        GM.InterfaceController = interfaceController;
    }

    public static InterfaceController GetInterfaceController => GM.InterfaceController;

    private Camera playerMainCamera;

    public static Camera PlayerMainCamera
    {
        get
        {
            if (instance.playerMainCamera == null)
            {
                instance.playerMainCamera = Camera.main;
            }
            return instance.playerMainCamera;
        }
        set
        {
            instance.playerMainCamera = value;
        }
    }

    //public static bool IsPlayerAlive
    //{
    //    get
    //    {
    //        return GetPlayerComponent<HealthController>().IsAlive;
    //    }
    //}

    private static GameManager GM
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private static GameManager instance;

    public static Vector3 PlayerLookDirection
    {
        get
        {
            return Camera.main.transform.forward;
        }
    }


    public static void FreezeCamera()
    {
        blockCameraMovement.Add(instance);
    }

    public static void FreezePlayer()
    {
        FreezeCamera();
        DisablePlayerMovement();
        DisableAllPlayerActions();
    }

    public static void UnfreezePlayer()
    {
        UnfreezeCamera();
        EnablePlayerMovement();
        EnablePlayerActions();
    }

    public static void UnfreezeCamera()
    {
        blockCameraMovement.Remove(instance);
    }

    public static bool CanCameraMove
    {
        get
        {
            return GameIsNotFrozen && blockCameraMovement.Count == 0;
        }
    }

    public static void FreezeGame()
    {
        GM.isGameFrozen = true;
        Time.timeScale = 0;
    }

    public static void UnfreezeGame()
    {
        GM.isGameFrozen = false;
        Time.timeScale = 1;
    }

    public static bool GameIsNotFrozen
    {
        get
        {
            return !GM.isGameFrozen;
        }
    }

    public static void DisablePlayerMovement()
    {
        blockPlayerMovement.Add(instance);
    }

    public static void EnablePlayerMovement()
    {
        blockPlayerMovement.Remove(instance);
    }

    public static bool AllowPlayerMovement
    {
        get
        {
            return GameIsNotFrozen && blockPlayerMovement.Count == 0;
        }
    }

    public static bool AllowCameraMovement
    {
        get
        {
            return GameIsNotFrozen && blockCameraMovement.Count == 0;
        }
    }

    public static void DisableAllPlayerActions()
    {
        blockPlayerActiveAction.Add(instance);
        blockPlayerPassiveAction.Add(instance);
    }

    public static void EnablePlayerActions()
    {
        blockPlayerActiveAction.Remove(instance);
        blockPlayerPassiveAction.Remove(instance);
    }

    public static bool AllowPlayerActiveActions
    {
        get
        {
            return GameIsNotFrozen && Player.IsMyTurn && blockPlayerActiveAction.Count == 0;
        }
    }

    public static bool AllowPlayerPassiveActions
    {
        get
        {
            return GameIsNotFrozen && blockPlayerPassiveAction.Count == 0;
        }
    }

}
