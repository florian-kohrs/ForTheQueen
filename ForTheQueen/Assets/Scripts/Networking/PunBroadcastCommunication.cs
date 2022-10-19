using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunBroadcastCommunication : MonoBehaviourPun
{

    private void Awake()
    {
        Instance = this;
    }

    public static PunBroadcastCommunication Instance { get; private set; }

    public HexagonWorld world;

    public GameState gameState;

    public static void SafeRPC(string name, RpcTarget target, Action callIfNotConnected, params object[] parameters)
    {
        Broadcast.SafeRPC(Instance.photonView, name, target, callIfNotConnected, parameters);
    }

    [PunRPC]
    public void StartFight()
    {
        InterfaceController.GetInterfaceMask<PreBattleUI>().Fight();
    }

    [PunRPC]
    public void SneakFromFight()
    {
        InterfaceController.GetInterfaceMask<PreBattleUI>().Sneak();
    }

    [PunRPC]
    public void RetreatFromFight()
    {
        InterfaceController.GetInterfaceMask<PreBattleUI>().Retreat();
    }

    [PunRPC]
    public void EndCurrentPlayersTurn()
    {
        gameState.EndCurrentHerosTurn();
    }

    [PunRPC]
    public void UseFocus()
    {
        InterfaceController.GetInterfaceMask<SkillCheckUI>().UseFocus();
    }

    public static void UseFocusRPC()
    {
        SafeRPC(nameof(UseFocus), RpcTarget.All, Instance.UseFocus);
    }

    [PunRPC]
    public void RemoveUsedFocusRPC()
    {
        InterfaceController.GetInterfaceMask<SkillCheckUI>().RemoveFocus();
    }

    public static void RemoveUsedFocus()
    {
        SafeRPC(nameof(RemoveUsedFocusRPC), RpcTarget.All, Instance.RemoveUsedFocusRPC);
    }

    [PunRPC]
    public void StartCombatActionRPC(int index)
    {
        InterfaceController.GetInterfaceMask<CombatActionUI>().StartAction(index);
    }

    public static void StartCombatAction(int index)
    {
        SafeRPC(nameof(StartCombatActionRPC), RpcTarget.All,()=>Instance.StartCombatActionRPC(index), index);
    }

}
