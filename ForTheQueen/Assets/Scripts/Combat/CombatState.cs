using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatState : MonoBehaviourPun
{

    protected const int COMBAT_ROUND_CHUNK_SIZE = 20;

    public BattleMap battleMap;

    public List<IBattleParticipant> heroParticipants;

    public List<IBattleParticipant> enemyParticipants;

    protected int TotalParticipants => heroParticipants.Count + enemyParticipants.Count;

    public SortedList<float, IBattleParticipant> actionTimeline;

    public void StartCombat(BattleParticipants participants)
    {
        heroParticipants = participants.onPlayersSide.Select(p => p.GetParticipant()).ToList();
        enemyParticipants = participants.onEnemiesSide.Select(p => p.GetParticipant()).ToList();
        actionTimeline = new SortedList<float, IBattleParticipant>();
        CreateTimeLine(0);
    }

    protected void CreateTimeLine(int startIndex)
    {
        float keyExtra = 0f;
        foreach (IBattleParticipant participant in heroParticipants.Concat(enemyParticipants))
        {
            participant.CombatState = this;
            float turnCost = CombatSpeedCost(participant);
           
            for (int i = 0; i < COMBAT_ROUND_CHUNK_SIZE; i++)
            {
                actionTimeline.Add(turnCost * (i + startIndex) + keyExtra, participant);
            }
            keyExtra += 0.1f;
        }
    }

    protected int CombatSpeedCost(IBattleParticipant p) => 150 - p.Speed;

    public void EndTurn()
    {
        Broadcast.SafeRPC(photonView, nameof(EndTurnRPC), RpcTarget.All, EndTurn);
    }

    [PunRPC]
    protected void EndTurnRPC()
    {

    }

    protected void NextTurn()
    {
        IBattleParticipant b = actionTimeline[0];
        actionTimeline.RemoveAt(0);
        b.StartTurn();
    }

}
