using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatState : MonoBehaviourPun
{

    protected const int COMBAT_ROUND_CHUNK_SIZE = 20;

    public BattleMap battleMap;

    public ICollection<IBattleParticipant> battleParticipants;

    protected int TotalParticipants => battleParticipants.Count;

    public SortedList<float, IBattleParticipant> actionTimeline;

    public static BattleParticipants participants;

    private void Start()
    {
        StartCombat(participants);
    }

    public void StartCombat(BattleParticipants participants)
    {
        battleParticipants = battleMap.BeginBattle(participants);
        actionTimeline = new SortedList<float, IBattleParticipant>();
        CreateTimeLine(0);
        NextTurn();
    }

    protected void CreateTimeLine(int startIndex)
    {
        float keyExtra = 0f;
        foreach (IBattleParticipant participant in battleParticipants)
        {
            participant.CombatState = this;
            float turnCost = CombatSpeedCost(participant);
           
            for (int i = 1; i < COMBAT_ROUND_CHUNK_SIZE; i++)
            {
                actionTimeline.Add(turnCost * (i + startIndex) + keyExtra, participant);
            }
            keyExtra += 0.1f;
        }
    }

    protected int CombatSpeedCost(IBattleParticipant p) => 150 - p.Speed;

    public void EndTurn()
    {
        Broadcast.SafeRPC(photonView, nameof(EndTurnRPC), RpcTarget.All, EndTurnRPC);
    }

    [PunRPC]
    protected void EndTurnRPC()
    {
        NextTurn();
    }

    protected void NextTurn()
    {
        IBattleParticipant b = actionTimeline.Values[0];
        actionTimeline.RemoveAt(0);
        b.StartTurn();
    }

}
