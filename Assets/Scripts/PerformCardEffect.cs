using UnityEngine;
using Libraries;

public class PerformCardEffect
{
  CardName _cardName;
  GameObject _meHealthBar;
  GameObject _themHealthBar;
  GameObject _meArmourCounter;
  GameObject _themArmourCounter;
  AgentState _meState;
  AgentState _themState;

  public PerformCardEffect(CardName cardName, GameObject meHealthBar, GameObject themHealthBar, GameObject meArmourCounter, GameObject themArmourCounter, AgentState meState, AgentState themState)
  {
    _cardName = cardName;
    _meHealthBar = meHealthBar;
    _themHealthBar = themHealthBar;
    _meArmourCounter = meArmourCounter;
    _themArmourCounter = themArmourCounter;
    _meState = meState;
    _themState = themState;
  }

  public ICardOutcome Perform()
  {
    return CardLibrary.Cards[_cardName].Perform(_meState, _themState);
  }

  public void ResolveSpinner(SpinnerResult result)
  {
    CardLibrary.Cards[_cardName].Resolve(result, _meState, _themState);
    
    _meHealthBar.GetComponent<HealthBarController>().SetHealth(_meState.Health());
    _themHealthBar.GetComponent<HealthBarController>().SetHealth(_themState.Health());
    
    _meArmourCounter.GetComponent<ArmourCounterController>().SetArmour(_meState.Armour());
    _themArmourCounter.GetComponent<ArmourCounterController>().SetArmour(_themState.Armour());
  }
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
