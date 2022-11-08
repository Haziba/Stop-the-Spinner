using System.Linq;
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
    SetPlayer();
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
    _context.Get<GameObject>(ContextObjects.PlayerHand).GetComponent<HandController>().SetDeck(Player.Deck.ToArray(), Player.MaxCardsInHand);

    var enemy = Monster();

    _context.Get<GameObject>(ContextObjects.EnemyHand).GetComponent<HandController>().SetDeck(enemy.Deck, enemy.MaxCardsInHand);
    
    ChangeGameState(GameState.PlayerTurn);
  }

  void SetPlayer()
  {
    _context.Get<GameObject>(ContextObjects.PlayerHealthBar).GetComponent<HealthBarController>().SetHealth(Player.Health);
    _context.Get<GameObject>(ContextObjects.PlayerArmourCounter).GetComponent<ArmourCounterController>().Init(Player.Armour);
    _context.Get<GameObject>(ContextObjects.PlayerManaCounter).GetComponent<ManaCounterController>().Init(Player.MaxMana);
  }
  
  void SetEnemy()
  {
    _context.Get<GameObject>(ContextObjects.Enemy).GetComponent<MonsterController>().Init(MonsterName());
    _context.Get<GameObject>(ContextObjects.EnemyHealthBar).GetComponent<HealthBarController>().SetHealth(Monster().Health);
    _context.Get<GameObject>(ContextObjects.EnemyArmourCounter).GetComponent<ArmourCounterController>().Init(Monster().Armour);
    _context.Get<GameObject>(ContextObjects.EnemyManaCounter).GetComponent<ManaCounterController>().Init(Monster().Mana);
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
  
  //todo: Hmm not sure this belongs
  void SetBackground()
  {
    var config = _context.Get<IContextObject>(ContextObjects.EnemyConfig) as EnemyConfig;
    _context.Get<GameObject>(ContextObjects.Background).GetComponent<BackgroundController>().SetImage(config.BackgroundName);
  }
}
