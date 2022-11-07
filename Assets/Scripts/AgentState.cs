using System;
using System.Collections.Generic;
using System.Linq;
using Libraries;
using UnityEngine;

public class AgentState : IContextObject
{
  IList<StatusEffect> _statusEffects;
  int _health;
  int _armour;

  int _initialMana;
  int _mana;
  int _manaRecoveryRate;

  public AgentState(int health, int armour, int mana, int manaRecoveryRate)
  {
    _statusEffects = new List<StatusEffect>();
    _health = health;
    _armour = armour;
    _initialMana = mana;
    _mana = mana;
    _manaRecoveryRate = manaRecoveryRate;

    Debug.Log("Mana: " + _mana);
  }

  public void NextTurn()
  {
    for(var i = _statusEffects.Count-1; i >= 0; i--)
    {
      _statusEffects[i].NextTurn();
      if(_statusEffects[i].Finished())
        _statusEffects.RemoveAt(i);
    }
  }

  public void TakeDamage(int damage)
  {
    if (damage < _armour)
    {
      _armour -= damage;
      return;
    }
    
    _health -= damage - _armour;
    _armour = 0;
  }

  public int Health()
  {
    return _health;
  }

  public void SetArmour(int armour)
  {
    _armour = armour;
  }
  
  public int Armour()
  {
    return _armour;
  }

  public void AddEffect(AgentStatusEffects statusEffect, int turns)
  {
    _statusEffects.Add(new StatusEffect(statusEffect, turns));
  }

  public int Mana()
  {
    return _mana;
  }

  public void ResetMana()
  {
    _mana = _initialMana;
  }

  public void SpendMana(int mana)
  {
    Debug.Log("Have " + _mana + ", Spend " + mana);
    _mana -= mana;
  }

  public AgentStatusEffects StatusEffects()
  {
    var effects = _statusEffects.Select(sE => sE.Effect());
    if(effects.Any())
      return effects.Aggregate((one, two) => one | two);
    return default(AgentStatusEffects);
  }

  class StatusEffect
  {
    AgentStatusEffects _effect;
    int _turns;

    public StatusEffect(AgentStatusEffects effect, int turns)
    {
      _effect = effect;
      _turns = turns;
    }

    public AgentStatusEffects Effect()
    {
      return _effect;
    }

    public void NextTurn()
    {
      _turns--;
    }

    public bool Finished()
    {
      return _turns <= 0;
    }
  }

  public bool Alive()
  {
    return _health > 0;
  }

  public bool CanPlayCard(CardName cardName)
  {
    var enoughMana = Mana() >= CardLibrary.Cards[cardName].ManaCost;
    
    return enoughMana;
  }

  public void RecoverMana()
  {
    _mana = Math.Min(_mana + _manaRecoveryRate, _initialMana);
  }
}

[Flags]
public enum AgentStatusEffects
{
  Intoxicated = 1,
  Distracted = 2,
  Focused = 4,
}
