using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heroes
{

    public static void Reset()
    {
        heroes = new Hero[NUMBER_PLAYERS];
    }

    public const int NUMBER_PLAYERS = 3;

    protected static Hero[] heroes = new Hero[NUMBER_PLAYERS];

    public static Hero GetHeroFromID(int id) => heroes[id];

    public static Hero GetCurrentActiveHero()
    {
        return heroes.Where(h => h.IsHerosTurn).FirstOrDefault();
    }

    public static bool AllHeroesInitialized => heroes.All(h => h != null && h.customization.customizationDone);

    public static void SetHero(Hero h, int index)
    {
        heroes[index] = h;
    }

    public static void SpawnHeros(MapTile startTile)
    {
        foreach (var hero in heroes)
        {
            startTile.AddTileOccupation(hero);
        }
    }

}
