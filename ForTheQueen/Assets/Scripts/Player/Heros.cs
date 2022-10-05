using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heros : MonoBehaviour
{

    public const int NUMBER_PLAYERS = 3;

    protected static Hero[] players;

    public static Hero GetHeroFromID(int id) => players[id];

    public SpawnableCreature heroPrefab;

    public void LoadHeros(Hero[] players)
    {
        Heros.players = players;
    }

    private void Awake()
    {
        players = new Hero[NUMBER_PLAYERS];
        for (int i = 0; i < NUMBER_PLAYERS; i++)
        {
            players[i] = new Hero(heroPrefab);
        }
        LoadHeros(players);
    }

    public static void SpawnHeros(MapTile startTile)
    {
        foreach (var player in players)
        {
            startTile.AddTileOccupation(player);
        }
    }

}
