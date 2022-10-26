using Libraries;
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
    SetBackground();
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
    }, 5);

    var enemy = Monster();

    _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>().SetDeck(enemy.Deck, enemy.MaxCardsInHand);
    
    ChangeGameState(GameState.PlayerTurn);
  }

  void SetEnemy()
  {
    SetEnemyImage(MonsterName());
    _context.Get<GameObject>(ContextObjects.EnemyHealthBar).GetComponent<HealthBarController>().SetHealth(Monster().Health);
  }

  public void SetEnemyImage(MonsterName monsterName)
  {
    HideAllEnemyImages();
    ShowEnemyImage(monsterName);
  }

  void HideAllEnemyImages()
  {
    for(var i = 0; i < _context.Get<GameObject>(ContextObjects.Enemy).transform.childCount; i++){
      _context.Get<GameObject>(ContextObjects.Enemy).transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowEnemyImage(MonsterName monsterName)
  {
    _context.Get<GameObject>(ContextObjects.Enemy).transform.Find(monsterName.ToString()).gameObject.SetActive(true);;
  }

  MonsterName MonsterName()
  {
    var monsterConfig = _context.Get<IContextObject>(ContextObjects.EnemyConfig) as EnemyConfig;
    return monsterConfig.Name;
  }

  Monster Monster()
  {
    return MonsterLibrary.Monsters[MonsterName()];
  }
  
  void SetBackground()
  {
    var config = _context.Get<IContextObject>(ContextObjects.EnemyConfig) as EnemyConfig;
    _context.Get<GameObject>(ContextObjects.Background).GetComponent<BackgroundController>().SetImage(config.BackgroundName);
  }
}
