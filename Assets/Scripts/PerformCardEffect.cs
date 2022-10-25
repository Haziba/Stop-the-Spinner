using UnityEngine;
using System.Collections.Generic;

public class PerformCardEffect
{
  CardName _cardName;
  GameObject _meHealthBar;
  GameObject _themHealthBar;
  AgentState _meState;
  AgentState _themState;

  public PerformCardEffect(CardName cardName, GameObject meHealthBar, GameObject themHealthBar, AgentState meState, AgentState themState)
  {
    _cardName = cardName;
    _meHealthBar = meHealthBar;
    _themHealthBar = themHealthBar;
    _meState = meState;
    _themState = themState;
  }

  public ICardOutcome Perform()
  {
    switch(_cardName)
    {
      case CardName.SwordThem:
        return new SpinWheelOutcome(SpinnerConfig(CardName.SwordThem));
      case CardName.AxeThem:
        return new SpinWheelOutcome(SpinnerConfig(CardName.AxeThem));
      case CardName.DaggerThem:
        return new SpinWheelOutcome(SpinnerConfig(CardName.DaggerThem));
      case CardName.BiteThem:
        return new SpinWheelOutcome(SpinnerConfig(CardName.BiteThem));
      case CardName.DistractThem:
        _themState.AddEffect(AgentStatusEffects.Distracted, 2);
        return new NoOutcome();
      case CardName.FocusMe:
        _meState.AddEffect(AgentStatusEffects.Focused, 2);
        return new NoOutcome();
      case CardName.IntoxicateThem:
        _themState.AddEffect(AgentStatusEffects.Intoxicated, 2);
        return new NoOutcome();
    }

    return new NoOutcome();
  }

  SpinnerConfiguration SpinnerConfig(CardName cardName)
  {
    switch(cardName)
    {
      case CardName.SwordThem:
        return new SpinnerConfiguration(0.5f, 0.1f);
      case CardName.AxeThem:
        return new SpinnerConfiguration(0.4f, 0.2f); 
      case CardName.DaggerThem:
        return new SpinnerConfiguration(0.6f, 0.03f);
      case CardName.BiteThem:
        return new SpinnerConfiguration(0.35f, 0.1f);
      default:
        return new SpinnerConfiguration(0.5f, 0.1f);
    }
  }

  // todo: This shouldn't live here lol
  IDictionary<CardName, CardDamage> _cardDamages = new Dictionary<CardName, CardDamage>
  {
    [CardName.SwordThem] = new CardDamage(1, 2),
    [CardName.AxeThem] = new CardDamage(2, 3),
    [CardName.DaggerThem] = new CardDamage(1, 2),
    [CardName.BiteThem] = new CardDamage(2, 4),
  };

  public void ResolveSpinner(SpinnerResult result)
  {
    if(result == SpinnerResult.Hit) {
      _themState.TakeDamage(_cardDamages[_cardName].Hit);
      _themHealthBar.GetComponent<HealthBarController>().SetHealth(_themState.Health());
    }
    if(result == SpinnerResult.Crit) {
      _themState.TakeDamage(_cardDamages[_cardName].Crit);
      _themHealthBar.GetComponent<HealthBarController>().SetHealth(_themState.Health());
    }
  }
}

// todo: Move all these classes to sensible locations, maybe scoped to a folder / namespace
public class CardDamage
{
  int _hit;
  int _crit;

  public CardDamage(int hit, int crit)
  {
    _hit = hit;
    _crit = crit;
  }

  public int Hit => _hit;
  public int Crit => _crit;
}

public interface ICardOutcome
{
  EffectOutcome Outcome();
  object Data();
}

public class NoOutcome : ICardOutcome
{
  public EffectOutcome Outcome() { return EffectOutcome.Continue; }
  public object Data() { return null; }
}

public class SpinWheelOutcome : ICardOutcome
{
  SpinnerConfiguration _config;

  public SpinWheelOutcome(SpinnerConfiguration config)
  {
    _config = config;
  }

  public EffectOutcome Outcome() { return EffectOutcome.SpinWheel; }
  public object Data() { return _config; }
}

public enum EffectOutcome
{
  Continue,
  SpinWheel,
}
