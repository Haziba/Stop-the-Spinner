using Libraries;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleStateController  : StateController
{
  public EndBattleStateController(ContextManager context) : base(context)
  {
  }

  public override void Start()
  {
    SceneManager.LoadScene("WorldPathScene");
  }

  public override void Update()
  {
  }

  public override void End()
  {
  }
}
