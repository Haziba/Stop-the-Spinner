using System.Collections;
using System.Collections.Generic;
using Libraries;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
  GameState? _gameState;
  IDictionary<GameState, IStateController> _stateControllers;

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

  public GameObject Enemy;

  // Start is called before the first frame update
  void Start()
  {
    var sceneData = SceneDataHandler.GetData();

    var context = new ContextManager(
      new Dictionary<ContextObjects, GameObject> {
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
      },
      new Dictionary<ContextObjects, Camera>(),
      new Dictionary<ContextObjects, IContextObject>
      {
        [ContextObjects.PlayerState] = new AgentState(10),
        [ContextObjects.EnemyState] = new AgentState(10),
        [ContextObjects.EnemyConfig] = sceneData != null ? (EnemyConfig)sceneData[SceneDataKey.Enemy] : new EnemyConfig(MonsterName.Sleppy, WorldPathBackgroundName.OneWayPath),
      }
    );

    _stateControllers = new Dictionary<GameState, IStateController> {
      [GameState.StartBattle] = new StartBattleStateController(context),
      [GameState.PlayerTurn] = new PlayerTurnStateController(context),
      [GameState.EnemyTurn] = new EnemyTurnStateController(context),
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
