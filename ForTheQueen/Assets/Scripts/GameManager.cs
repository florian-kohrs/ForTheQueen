using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerPun
{

    static GameManagerPun()
    {
        SceneManager.activeSceneChanged += delegate
        {
            instance = new GameManagerPun();
        };
    }

    private GameManagerPun()
    {
        Time.timeScale = 1;
    }

    private bool isGameFrozen;

    private bool isMovementBlocked;

    private bool isPlayerMovementBlocked;
    
    private bool isActionBlocked;

    private bool isPlayerActionBlocked;

    private bool isCameraBlocked;

    private GameObject player;

    private InterfaceController interfaceController;

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
            if(instance.playerMainCamera == null)
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

    private static GameManagerPun GM
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManagerPun();
            }
            return instance;
        }
    }

    private static GameManagerPun instance;

    public static void ResetPlayerRef()
    {
        GM.player = null;
    }

    public static Vector3 PlayerLookDirection
    {
        get
        {
            return Camera.main.transform.forward;
        }
    }


    public static GameObject Player
    {
        get
        {
            if(GM.player == null)
            {
                GM.player = GameObject.FindGameObjectWithTag("Player");
            }
            return GM.player;
        }
    }

    public static T GetPlayerComponent<T>() where T: Component
    {
        return Player?.GetComponentInChildren<T>();
    }


    public static void FreezeCamera()
    {
        GM.isCameraBlocked = true;
    }

    public static void FreezePlayer()
    {
        FreezeCamera();
        DisablePlayerMovement();
        DisablePlayerActions();
    }

    public static void UnfreezePlayer()
    {
        UnfreezeCamera();
        EnablePlayerMovement();
        EnablePlayerActions();
    }

    public static void UnfreezeCamera()
    {
        GM.isCameraBlocked = false;
    }

    public static bool CanCameraMove
    {
        get
        {
            return GameIsNotFrozen && !GM.isCameraBlocked;
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
        GM.isPlayerMovementBlocked = true;
    }

    public static void EnablePlayerMovement()
    {
        GM.isPlayerMovementBlocked = false;
    }

    public static bool AllowPlayerMovement
    {
        get
        {
            return GameIsNotFrozen && !GM.isPlayerMovementBlocked;
        }
    }

    public static void DisableActions()
    {
        GM.isActionBlocked = true;
    }

    public static void EnableActions()
    {
        GM.isActionBlocked = false;
    }

    public static bool AllowActions
    {
        get
        {
            return GameIsNotFrozen && !GM.isActionBlocked;
        }
    }

    public static void DisablePlayerActions()
    {
        GM.isPlayerActionBlocked = true;
    }

    public static void EnablePlayerActions()
    {
        GM.isPlayerActionBlocked = false;
    }

    public static bool AllowPlayerActions
    {
        get
        {
            return GameIsNotFrozen && !GM.isPlayerActionBlocked;
        }
    }

}
