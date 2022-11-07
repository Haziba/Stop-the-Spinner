using System.Collections;
using System.Collections.Generic;
using Libraries;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  GameState? _gameState;
  IDictionary<GameState, IStateController> _stateControllers;

  public GameObject Instantiator;
  
  public GameObject Background;

  public GameObject PlayerHealthBar;
  public GameObject EnemyHealthBar;

  public GameObject PlayerSpinner;
  public GameObject EnemySpinner;

  public GameObject PlayerHand;
  public GameObject EnemyHand;

  public GameObject PlayerPlayedCard;
  public GameObject EnemyPlayedCard;

  public GameObject PlayerDrawPile;
  public GameObject PlayerDiscardPile;

  public GameObject EnemyDrawPile;
  public GameObject EnemyDiscardPile;

  public GameObject PlayerManaCounter;
  public GameObject EnemyManaCounter;

  public GameObject PlayerArmourCounter;
  public GameObject EnemyArmourCounter;

  public GameObject Enemy;

  public GameObject DamageBallPrefab;
  public GameObject PlayerDamageAnchor;

  public Camera Camera;

  // Start is called before the first frame update
  void Start()
  {
    var sceneData = SceneDataHandler.GetData() ?? new Dictionary<SceneDataKey, object>
    {
      [SceneDataKey.Enemy] = new EnemyConfig(MonsterName.Trupple, BackgroundName.DeadEnd)
    };

    var monster = MonsterLibrary.Monsters[((EnemyConfig)sceneData[SceneDataKey.Enemy]).Name];

    var context = new ContextManager(
      new Dictionary<ContextObjects, GameObject> {
        [ContextObjects.Instantiator] = Instantiator,
        
        [ContextObjects.PlayerHealthBar] = PlayerHealthBar,
        [ContextObjects.EnemyHealthBar] = EnemyHealthBar,

        [ContextObjects.PlayerSpinner] = PlayerSpinner,
        [ContextObjects.EnemySpinner] = EnemySpinner,

        [ContextObjects.PlayerHand] = PlayerHand,
        [ContextObjects.EnemyHand] = EnemyHand,

        [ContextObjects.PlayerPlayedCard] = PlayerPlayedCard,
        [ContextObjects.EnemyPlayedCard] = EnemyPlayedCard,

        [ContextObjects.Enemy] = Enemy,
        
        [ContextObjects.Background] = Background,

        [ContextObjects.PlayerDrawPile] = PlayerDrawPile,
        [ContextObjects.PlayerDiscardPile] = PlayerDiscardPile,

        [ContextObjects.EnemyDrawPile] = EnemyDrawPile,
        [ContextObjects.EnemyDiscardPile] = EnemyDiscardPile,
        
        [ContextObjects.PlayerManaCounter] = PlayerManaCounter,
        [ContextObjects.EnemyManaCounter] = EnemyManaCounter,
        
        [ContextObjects.PlayerArmourCounter] = PlayerArmourCounter,
        [ContextObjects.EnemyArmourCounter] = EnemyArmourCounter,
        
        [ContextObjects.DamageBallPrefab] = DamageBallPrefab,
        [ContextObjects.PlayerDamageAnchor] = PlayerDamageAnchor,
      },
      new Dictionary<ContextObjects, Camera>
      {
        [ContextObjects.Camera] = Camera,
      },
      new Dictionary<ContextObjects, IContextObject>
      {
        [ContextObjects.PlayerState] = new AgentState(10, 0, 3, 2),
        [ContextObjects.EnemyState] = new AgentState(monster.Health, monster.Armour, monster.Mana, monster.ManaRecoveryRate),
        [ContextObjects.EnemyConfig] = sceneData != null ? (EnemyConfig)sceneData[SceneDataKey.Enemy] : new EnemyConfig(MonsterName.Sleppy, BackgroundName.OneWayPath),
      }
    );

    _stateControllers = new Dictionary<GameState, IStateController> {
      [GameState.StartBattle] = new StartBattleStateController(context),
      [GameState.PlayerTurn] = new PlayerTurnStateController(context),
      [GameState.EnemyTurn] = new EnemyTurnStateController(context),
      [GameState.EndBattle] = new EndBattleStateController(context),
    };

    SwitchGameState(GameState.StartBattle);
  }

  // Update is called once per frame
	void Update()
	{
    CurrentGameState().Update();
    var newGameState = CurrentGameState().CurrentGameState();
    if(newGameState != _gameState)
      SwitchGameState(newGameState);
	}

  void SwitchGameState(GameState newGameState)
  {
    if(_gameState != null) {
      CurrentGameState().SetActive(false);
      CurrentGameState().End();
    }
    _gameState = newGameState;
    CurrentGameState().SetActive(true);
    CurrentGameState().Start();
  }

  IStateController CurrentGameState()
  {
    return _stateControllers[_gameState.Value];
  }
}
