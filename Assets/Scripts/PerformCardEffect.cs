using UnityEngine;

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

  public EffectOutcome Perform()
  {
    switch(_cardName)
    {
      case CardName.SwordThem:
        return EffectOutcome.SpinWheel;
      case CardName.AxeThem:
        return EffectOutcome.SpinWheel;
      case CardName.DistractThem:
        _themState.AddEffect(AgentStatusEffects.Distracted, 2);
        return EffectOutcome.Continue;
      case CardName.FocusMe:
        _meState.AddEffect(AgentStatusEffects.Focused, 2);
        return EffectOutcome.Continue;
      case CardName.IntoxicateThem:
        _themState.AddEffect(AgentStatusEffects.Intoxicated, 2);
        return EffectOutcome.Continue;
    }

    return EffectOutcome.Continue;
  }

  public void ResolveSpinner(SpinnerResult result)
  {
    if(result == SpinnerResult.Hit) {
      _themState.TakeDamage(1);
      _themHealthBar.GetComponent<HealthBarController>().SetHealth(_themState.Health());
    }
    if(result == SpinnerResult.Crit) {
      _themState.TakeDamage(1);
      _themHealthBar.GetComponent<HealthBarController>().SetHealth(_themState.Health());
    }
  }
}

public enum EffectOutcome
{
  Continue,
  SpinWheel,
}
