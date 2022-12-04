using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libraries
{
  public static class CardLibrary
  {
    public static IDictionary<CardName, Card> Cards = new Dictionary<CardName, Card>
    {
      [CardName.SwordThem] = new Card(2, (meState, themState) =>
      {
        return new SpinWheelOutcome(new SpinnerConfiguration(0.5f, 0.1f));
      }, new DamageToolTip(1, 2), 
        (spinnerResult, meState, themState) =>
      {
        switch (spinnerResult)
        {
          case SpinnerResult.Hit:
            themState.TakeDamage(2);
            break;
          case SpinnerResult.Crit:
            themState.TakeDamage(3);
            break;
        }
      }),
      [CardName.AxeThem] = new Card(3, (meState, themState) =>
      {
        return new SpinWheelOutcome(new SpinnerConfiguration(0.4f, 0.2f));
      }, new DamageToolTip(2, 3), 
        (spinnerResult, meState, themState) =>
      {
        switch (spinnerResult)
        {
          case SpinnerResult.Hit:
            themState.TakeDamage(2);
            break;
          case SpinnerResult.Crit:
            themState.TakeDamage(4);
            break;
        }
      }),
      [CardName.DaggerThem] = new Card(1, (meState, themState) =>
      {
        return new SpinWheelOutcome(new SpinnerConfiguration(0.6f, 0.03f));
      }, new DamageToolTip(1, 3), 
        (spinnerResult, meState, themState) =>
      {
        switch (spinnerResult)
        {
          case SpinnerResult.Hit:
            themState.TakeDamage(1);
            break;
          case SpinnerResult.Crit:
            themState.TakeDamage(3);
            break;
        }
      }), 
      [CardName.BiteThem] = new Card(3, (meState, themState) =>
      {
        return new SpinWheelOutcome(new SpinnerConfiguration(0.35f, 0.1f));
      }, new DamageToolTip(2, 4), 
        (spinnerResult, meState, themState) =>
      {
        switch (spinnerResult)
        {
          case SpinnerResult.Hit:
            themState.TakeDamage(2);
            break;
          case SpinnerResult.Crit:
            themState.TakeDamage(4);
            break;
        }
      }),
      [CardName.DistractThem] = new Card(1, (meState, themState) =>
      {
        themState.AddEffect(AgentStatusEffects.Distracted, 2);
        return new NoOutcome();
      }, new StatusEffectToolTip(AgentStatusEffects.Distracted, false)), 
      [CardName.FocusMe] = new Card(1, (meState, themState) =>
      {
        meState.AddEffect(AgentStatusEffects.Focused, 2);
        return new NoOutcome();
      }, new StatusEffectToolTip(AgentStatusEffects.Focused, true)), 
      [CardName.IntoxicateThem] = new Card(2, (meState, themState) =>
      {
        themState.AddEffect(AgentStatusEffects.Intoxicated, 2);
        return new NoOutcome();
      }, new StatusEffectToolTip(AgentStatusEffects.Intoxicated, false)), 
      [CardName.ShieldBashThem] = new Card(2, (meState, themState) =>
      {
        return new SpinWheelOutcome(new SpinnerConfiguration(0.5f, 0.1f));
      }, new DamageToolTip(1, 2), 
        (spinnerResult, meState, themState) =>
      {
        switch (spinnerResult)
        {
          case SpinnerResult.Hit:
            themState.TakeDamage(1);
            break;
          case SpinnerResult.Crit:
            themState.TakeDamage(2);
            break;
        }
      }),
      [CardName.RaiseShield] = new Card(2, (meState, themState) =>
      {
        meState.SetArmour(meState.Armour() + 1);
        return new NoOutcome();
      }, new ArmourToolTip(1))
    };
  }
}

public class Card
{
  int _manaCost;
  Func<AgentState,AgentState,ICardOutcome> _perform;
  Action<SpinnerResult,AgentState,AgentState> _resolve;

  public int ManaCost => _manaCost;
  public Func<AgentState, AgentState, ICardOutcome> Perform => _perform;
  public Action<SpinnerResult, AgentState, AgentState> Resolve => _resolve;
  
  public IToolTip ToolTip { get; }

  public Card(int manaCost, Func<AgentState, AgentState, ICardOutcome> perform, IToolTip toolTip, Action<SpinnerResult, AgentState, AgentState> resolve = null)
  {
    _manaCost = manaCost;
    _perform = perform;
    ToolTip = toolTip;
    _resolve = resolve;
  }
}