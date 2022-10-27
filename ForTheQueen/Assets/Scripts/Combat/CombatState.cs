using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CombatState : MonoBehaviourPun
{

    protected const int COMBAT_ROUND_CHUNK_SIZE = 20;

    public BattleMap battleMap;

    public ICollection<IBattleParticipant> battleParticipants;

    protected int TotalParticipants => battleParticipants.Count;

    public SortedList<float, IBattleParticipant> actionTimeline;

    public static BattleParticipants participants;

    public BattleParticipants testParticipants = new BattleParticipants();

    protected Queue<GameObject> timelineObjects = new Queue<GameObject>();

    public SingleEnemyOccupationScripableObject enemy;

    protected IBattleParticipant activeParticipant;

    public IBattleParticipant ActiveParticipant => activeParticipant;

    public List<Vector2Int> FieldsWithHeroes => battleParticipants.Where(b => b.OnPlayersSide).Select(b => b.CurrentTile).ToList();

    [SerializeField]
    protected Transform timelineParent;

    [SerializeField]
    protected GameObject timeLinePrefab;

    [SerializeField]
    protected Button endTurnBtn;

    public void ParticipantDied(IBattleParticipant participant)
    {
        FilterTimeline(participant);
    }

    protected void FilterTimeline(IBattleParticipant p)
    {
        for (int i = 0; i < actionTimeline.Count; i++)
        {
            if (actionTimeline.Values[i] == p)
            { 
                actionTimeline.RemoveAt(i);
                i--;
            }
        }
    }

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
                CreateEnemyUI(e as NPCBattleParticipant);

        actionTimeline = new SortedList<float, IBattleParticipant>();
        CreateTimeLine(0);
        SpawnTimeLine();
        NextTurn();
    }

    protected void CreateEnemyUI(NPCBattleParticipant p)
    {
        GameObject g = Instantiate(enemyUIPrefab, enemyUIParent);
        EnemyBattleInfoUI e = g.GetComponent<EnemyBattleInfoUI>();
        p.enemyUI = e;
        e.ApplySingleEnemy(p);
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

    protected void SpawnTimeLine()
    {
        ClearTimeLine();
        foreach (var kv in actionTimeline)
        {
            GameObject uiObject = Instantiate(timeLinePrefab, timelineParent);
            kv.Value.AddUIReference(uiObject.GetComponent<TimelineUI>());
            timelineObjects.Enqueue(uiObject);
        }
    }

    protected void ClearTimeLine()
    {
        while (timelineObjects.Count > 0)
            Destroy(timelineObjects.Dequeue());
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
        endTurnBtn.interactable = activeParticipant.IsMine;
        Destroy(timelineObjects.Dequeue());
        actionTimeline.RemoveAt(0);
        activeParticipant.StartTurn();
    }

    public void BeginHoverMapTile(Vector2Int v2)
    {
        Broadcast.SafeRPC(photonView, nameof(RPCBeginHoverMapTile), RpcTarget.All, ()=>RPCBeginHoverMapTile(v2.x,v2.y), v2.x,v2.y);
    }

    [PunRPC]
    public void RPCBeginHoverMapTile(int x, int y)
    {
        battleMap.MarkActionOnMap(new Vector2Int(x,y), HeroCombat.currentHeroTurnInCombat.SelectedCombatAction);
    }

    public void StopHoveredTile()
    {
        Broadcast.SafeRPC(photonView, nameof(RPCStopHoveredTile), RpcTarget.All, RPCStopHoveredTile);
    }

    [PunRPC]
    public void RPCStopHoveredTile()
    {
        battleMap.RemovePreviousMarkers();
    }

    public void SelectHoveredMapTile(Vector2Int v2)
    {
        Broadcast.SafeRPC(photonView, nameof(RPCSelectHoveredMapTile), RpcTarget.All, () => RPCSelectHoveredMapTile(v2.x,v2.y), v2.x,v2.y);
    }

    [PunRPC]
    public void RPCSelectHoveredMapTile(int x, int y)
    {
        HeroCombat.currentHeroTurnInCombat.ExecuteSelectedAction(new Vector2Int(x,y));
    }

    public void NPCAttack(int actionIndex, Vector2Int v2)
    {
        Broadcast.SafeRPC(photonView, nameof(NPCAttackRPC), RpcTarget.All, () => NPCAttackRPC(actionIndex, v2.x, v2.y), actionIndex, v2.x,v2.y);
    }

    [PunRPC]
    public void NPCAttackRPC(int actionIndex, int x, int y)
    {
        activeParticipant.ExecuteAction(actionIndex, new Vector2Int(x,y));
    }
}
