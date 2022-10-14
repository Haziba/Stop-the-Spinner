using System.Collections.Generic;
using UnityEngine;

public abstract class StateController : IStateController
{
  GameState _currentGameState;
  protected ContextManager _context;

  bool _active;

  public StateController(ContextManager context)
  {
    _context = context;
  }

  public void SetActive(bool active) {
    _active = active;
  }

  public virtual void Start() {
  }

  public virtual void Update() {
  }

  public virtual void End() { 
  }

  public GameState CurrentGameState() {
    return _currentGameState;
  }

  public void ChangeGameState(GameState newGameState) {
    _currentGameState = newGameState;
  }

  protected bool IsActiveState() {
    return _active;
  }
}
