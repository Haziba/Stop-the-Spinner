using UnityEngine;

public class StartBattleStateController  : StateController
{
  public StartBattleStateController(ContextManager context) : base(context)
  {
  }

  public override void Start()
  {
    DealCards();
    SetEnemy();
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
        CardName.FocusMe,
        CardName.DistractThem,
        CardName.SwordThem,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.DistractThem
    });

    _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>().SetDeck(new CardName[] {
        CardName.AxeThem,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.DistractThem,
        CardName.AxeThem,
        CardName.AxeThem,
        CardName.FocusMe,
        CardName.DistractThem,
        CardName.IntoxicateThem,
    });

    ChangeGameState(GameState.PlayerTurn);
  }

  void SetEnemy()
  {
    var enemyName = (_context.Get<IContextObject>(ContextObjects.EnemyConfig) as EnemyConfig).Name;

    SetEnemyImage(enemyName);
  }

  public void SetEnemyImage(EnemyName enemyName)
  {
    HideAllEnemyImages();
    ShowEnemyImage(enemyName);
  }

  void HideAllEnemyImages()
  {
    for(var i = 0; i < _context.Get<GameObject>(ContextObjects.Enemy).transform.childCount; i++){
      _context.Get<GameObject>(ContextObjects.Enemy).transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowEnemyImage(EnemyName enemyName)
  {
    _context.Get<GameObject>(ContextObjects.Enemy).transform.Find(enemyName.ToString()).gameObject.SetActive(true);;
  }
}
