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
        Mana = 2,
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
        Mana = 1,
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
        Mana = 3,
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
      },
      [MonsterName.Witch] = new Monster
      {
        Health = 6,
        Mana = 3,
        Deck = new []
        {
          CardName.DaggerThem,
          CardName.DaggerThem,
          CardName.SwordThem,
          CardName.DistractThem,
          CardName.IntoxicateThem,
          CardName.IntoxicateThem,
        },
        MaxCardsInHand = 4
      },
      [MonsterName.Trupple] = new Monster
      {
        Health = 5,
        Armour = 5,
        Mana = 2,
        Deck = new []
        {
          CardName.ShieldBashThem,
          CardName.ShieldBashThem,
          CardName.ShieldBashThem,
          CardName.DistractThem,
          CardName.RaiseShield,
          CardName.RaiseShield,
        },
        MaxCardsInHand = 4,
        ExtraSprites = new []
        {
          new ExtraSprite("ArmourBar0", ResolutionType.Armour, ExtraSpriteType.RemoveOverlay, 0)
        }
      }
    };
  }

  public enum ExtraSpriteType
  {
    RemoveOverlay
  }

  public enum MonsterName
  {
    Gronk,
    Flaps,
    Sleppy,
    Witch,
    Trupple
  }

  public class Monster
  {
    public int Health { get; set;  }
    public int Mana { get; set; }
    public int Armour { get; set; }
    public CardName[] Deck { get; set; }
    public int MaxCardsInHand { get; set;  }
    public ExtraSprite[] ExtraSprites { get; set; }
    public ResolutionEventStep.Resolution[] Resolutions { get; set; }
    public int ManaRecoveryRate { get; set; } = 1;
  }

  public class ExtraSprite
  {
    public string Name { get; }
    public ResolutionType ResolutionType { get; }
    public ExtraSpriteType Type { get; }
    public int TriggerLimit { get; }

    bool _triggered;
    public bool Triggered => _triggered;

    public ExtraSprite(string name, ResolutionType resolutionType, ExtraSpriteType type, int triggerLimit)
    {
      Name = name;
      ResolutionType = resolutionType;
      Type = type;
      TriggerLimit = triggerLimit;
      _triggered = false;
    }

    public void Trigger()
    {
      _triggered = true;
    }
  }
}