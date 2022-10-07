using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heroes : MonoBehaviour
{

    public const int NUMBER_PLAYERS = 3;

    public static Hero[] heroes = new Hero[NUMBER_PLAYERS];

    public static Hero GetHeroFromID(int id) => heroes[id];

    public SpawnableCreature heroPrefab;

    public static Hero GetCurrentActiveHero()
    {
        return heroes.Where(h => h.IsHerosTurn).FirstOrDefault();
    }

    public static void SetHero(Hero h, int index)
    {
        heroes[index] = h;
    }

    public void LoadHeros(Hero[] heros)
    {
        Heroes.heroes = heros;
    }

    private void Awake()
    {
        if (heroes[1] != null)
            return;

        heroes = new Hero[NUMBER_PLAYERS];
        for (int i = 0; i < NUMBER_PLAYERS; i++)
        {
            heroes[i] = new Hero(heroPrefab, i);
        }
        LoadHeros(heroes);
    }

    public static void SpawnHeros(MapTile startTile)
    {
        foreach (var player in heroes)
        {
            startTile.AddTileOccupation(player);
        }
    }

}
