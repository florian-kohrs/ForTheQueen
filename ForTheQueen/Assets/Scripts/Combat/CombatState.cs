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

    public BattleParticipants testParticipants = new BattleParticipants();

    public SingleEnemyOccupationScripableObject enemy;

    protected IBattleParticipant activeParticipant;

    public GameObject enemyUIPrefab;

    public Transform enemyUIParent;

    public MouseToHovoredBattleMapTile mouseMapTileEvent;

    private void Start()
    {
        if (participants == null)
        {
            GameInstanceData.CurrentGameInstanceData.ShuffleGameInstanceSeed();
            EnterTest();
        }
        StartCombat(participants);
    }

    protected void EnterTest()
    {
        Heroes.CreateHeroes((HeroClass)GenerellLookup.instance.customizationLookup.classes.list[0]);
        testParticipants = new BattleParticipants();
        testParticipants.onPlayersSide = new List<IBattleOccupation>();
        testParticipants.onEnemiesSide = new List<IBattleOccupation>();
        testParticipants.onPlayersSide.AddRange(Heroes.AllHeroes);
        testParticipants.onEnemiesSide.Add(new SingleEnemyOccupation(enemy));
        participants = testParticipants;
    }

    public void StartCombat(BattleParticipants participants)
    {
        battleParticipants = battleMap.BeginBattle(participants);
        foreach (var e in battleMap.activeParticipants)
            if (!e.OnPlayersSide)
                CreateEnemyUI(e);

        actionTimeline = new SortedList<float, IBattleParticipant>();
        CreateTimeLine(0);
        NextTurn();
    }

    protected void CreateEnemyUI(IBattleParticipant p)
    {
        GameObject g = Instantiate(enemyUIPrefab, enemyUIParent);
        g.GetComponent<EnemyBattleInfoUI>().ApplySingleEnemy(p);
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
        if (activeParticipant != null)
            activeParticipant.OnTurnEnded();
        activeParticipant = actionTimeline.Values[0];
        actionTimeline.RemoveAt(0);
        activeParticipant.StartTurn();
    }

    public void BeginHoverMapTile(Vector2Int v2)
    {
        Broadcast.SafeRPC(photonView, nameof(RPCBeginHoverMapTile), RpcTarget.All, ()=>RPCBeginHoverMapTile(v2), v2);
    }

    [PunRPC]
    public void RPCBeginHoverMapTile(Vector2Int v2)
    {
        battleMap.MarkActionOnMap(v2, HeroCombat.currentHeroTurnInCombat.SelectedCombatAction);
    }

    public void StopHoveredTile(Vector2Int v2)
    {
        Broadcast.SafeRPC(photonView, nameof(RPCStopHoveredTile), RpcTarget.All, () => RPCStopHoveredTile(v2), v2);
    }

    [PunRPC]
    public void RPCStopHoveredTile(Vector2Int v2)
    {
        battleMap.RemovePreviousMarkers();
    }

    public void SelectHoveredMapTile(Vector2Int v2)
    {
        Broadcast.SafeRPC(photonView, nameof(RPCSelectHoveredMapTile), RpcTarget.All, () => RPCSelectHoveredMapTile(v2), v2);
    }

    [PunRPC]
    public void RPCSelectHoveredMapTile(Vector2Int v2)
    {
        HeroCombat.currentHeroTurnInCombat.ExecuteSelectedAction(v2);
    }

}
