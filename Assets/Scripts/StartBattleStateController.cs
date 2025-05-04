using System.Linq;
using Libraries;

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
    _context.GO(ContextObjects.PlayerHand).GetComponent<HandController>().SetDeck(Player.Instance.Deck.Select(card => card.CardName).ToArray(), Player.Instance.MaxCardsInHand);

    var enemy = Monster();

    _context.GO(ContextObjects.EnemyHand).GetComponent<HandController>().SetDeck(enemy.Deck, enemy.MaxCardsInHand);
    
    ChangeGameState(GameState.PlayerTurn);
  }

  void SetPlayer()
  {
    _context.GO(ContextObjects.PlayerHealthBar).GetComponent<HealthBarController>().SetHealthAndArmour(Player.Instance.Health, Player.Instance.Armour);
    _context.GO(ContextObjects.PlayerManaCounter).GetComponent<ManaCounterController>().Init(Player.Instance.MaxMana);
  }
  
  void SetEnemy()
  {
    _context.GO(ContextObjects.Enemy).GetComponent<MonsterController>().Init(MonsterName());
    _context.GO(ContextObjects.EnemyHealthBar).GetComponent<HealthBarController>().SetHealthAndArmour(Monster().Health, Monster().Armour);
    _context.GO(ContextObjects.EnemyManaCounter).GetComponent<ManaCounterController>().Init(Monster().Mana);
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
    _context.GO(ContextObjects.Background).GetComponent<BackgroundController>().SetImage(config.BackgroundName);
  }
}
