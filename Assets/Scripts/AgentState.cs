using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class AgentState : IContextObject
{
  IList<StatusEffect> _statusEffects;
  int _health;

  public AgentState(int health)
  {
    _statusEffects = new List<StatusEffect>();
    _health = health;
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
    _health -= damage;
  }

  public int Health()
  {
    return _health;
  }

  public void AddEffect(AgentStatusEffects statusEffect, int turns)
  {
    _statusEffects.Add(new StatusEffect(statusEffect, turns));
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
}

[Flags]
public enum AgentStatusEffects
{
  Intoxicated = 1,
  Distracted = 2,
  Focused = 4,
}
