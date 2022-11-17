public interface IToolTip
{
  string Text();
}

public class DamageToolTip : IToolTip
{
  readonly int _damage;
  readonly int _critDamage;

  public DamageToolTip(int damage, int critDamage)
  {
    _damage = damage;
    _critDamage = critDamage;
  }
  
  public string Text()
  {
    return $"Deals {_damage} damage on hit, and {_critDamage} on crit";
  }
}

public class ArmourToolTip : IToolTip
{
  readonly int _armour;

  public ArmourToolTip(int armour)
  {
    _armour = armour;
  }

  public string Text()
  {
    return $"Grants the user {_armour} armour";
  }
}

public class StatusEffectToolTip : IToolTip
{
  readonly AgentStatusEffects _statusEffect;
  readonly bool _effectsPlayer;

  public StatusEffectToolTip(AgentStatusEffects statusEffect, bool effectsPlayer)
  {
    _statusEffect = statusEffect;
    _effectsPlayer = effectsPlayer;
  }

  public string Text()
  {
    if (_effectsPlayer)
      return $"Applies {_statusEffect} to you";
    else
      return $"Applies {_statusEffect} to the enemy";
  }
}
