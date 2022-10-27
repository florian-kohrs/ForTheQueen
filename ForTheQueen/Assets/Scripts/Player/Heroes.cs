using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heroes
{

    public static void Reset()
    {
        heroes = new Hero[NUMBER_HEROES];
    }

    public const int NUMBER_HEROES = 3;

    protected static Hero[] heroes = new Hero[NUMBER_HEROES];

    public static IEnumerable<Hero> AllHeroes => heroes;

    public static Hero GetHeroFromID(int id) => heroes[id];

    public static Hero GetHeroWithActiveTurn()
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
        if (heroes[0] == null)
            CreateHeroes((HeroClass)GenerellLookup.instance.customizationLookup.classes.list[0]);
        foreach (var hero in heroes)
        {
            startTile.AddTileOccupation(hero);
        }
    }

    public static void CreateHeroes()
    {
        Reset();
        GameInstanceData.CurrentGameInstanceData.ShuffleGameInstanceSeed();
        for (int i = 0; i < NUMBER_HEROES; i++)
        {
            heroes[i] = new Hero() { heroIndex = i, heroStats = new CreatureStats() { speed = 60 } };
        }
    }

    public static void CreateHeroes(HeroClass heroClass)
    {
        Reset();
        //GameInstanceData.CurrentGameInstanceData.ShuffleGameInstanceSeed();
        for (int i = 0; i < NUMBER_HEROES; i++)
        {
            heroes[i] = new Hero() { heroIndex = i, maxFocus = 5, currentFocus = 5 };
            heroClass.Apply(null, heroes[i]);
        }
    }

}
