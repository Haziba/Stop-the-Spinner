using UnityEngine;

public class StartBattleStateController  : StateController
{
  public StartBattleStateController(ContextManager context) : base(context)
  {
  }

  public override void Start()
  {
    DealCards();
  }

  public override void Update()
  {
  }

  public override void End()
  {
  }

  public void DealCards()
  {
    _context.Get<GameObject>(ContextObjects.PlayerHand).GetComponent<HandController>().SetDeck(new CardName[] {
        CardName.SwordThem,
        CardName.SwordThem,
        CardName.AxeThem,
        CardName.AxeThem,
        CardName.IntoxicateThem,
        /*CardName.FocusMe,
        CardName.DistractThem,
        CardName.SwordThem,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.DistractThem*/
    });

    _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>().SetDeck(new CardName[] {
        CardName.AxeThem,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.DistractThem
    });

    ChangeGameState(GameState.PlayerTurn);
  }
}
