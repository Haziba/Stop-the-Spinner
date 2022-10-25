using System.Collections.Generic;

namespace Libraries
{
  public static class MonsterLibrary
  {
    public static IDictionary<MonsterName, Monster> Monsters = new Dictionary<MonsterName, Monster>
    {
      [MonsterName.Gronk] = new Monster
      {
        Health = 6,
        Deck = new[]
        {
          CardName.AxeThem,
          CardName.AxeThem,
          CardName.DistractThem,
          CardName.DistractThem,
          CardName.SwordThem,
          CardName.SwordThem
        },
        MaxCardsInHand = 3,
        Resolutions = new []
        {
          new ResolutionEventStep.Resolution(ResolutionType.Gold, 10)
        }
      },
      [MonsterName.Flaps] = new Monster
      {
        Health = 4,
        Deck = new []
        {
          CardName.DaggerThem,
          CardName.DaggerThem,
          CardName.DaggerThem,
          CardName.FocusMe,
          CardName.FocusMe,
          CardName.FocusMe,
        },
        MaxCardsInHand = 3,
        Resolutions = new []
        {
          new ResolutionEventStep.Resolution(ResolutionType.Gold, 5)
        }
      },
      [MonsterName.Sleppy] = new Monster
      {
        Health = 10,
        Deck = new []
        {
          CardName.BiteThem,
          CardName.BiteThem,
          CardName.BiteThem,
          CardName.BiteThem,
          CardName.FocusMe,
          CardName.FocusMe,
        },
        MaxCardsInHand = 4,
        Resolutions = new []
        {
          new ResolutionEventStep.Resolution(ResolutionType.GainCard, CardName.FocusMe)
        }
      }
    };
  }

  public enum MonsterName
  {
    Gronk,
    Flaps,
    Sleppy
  }

  public class Monster
  {
    public int Health { get; set;  }
    public CardName[] Deck { get; set; }
    public int MaxCardsInHand { get; set;  }
    public ResolutionEventStep.Resolution[] Resolutions { get; set; }
  }
}