public interface IStateController
{
  void SetActive(bool active);
  void Start();
  void Update();
  void End();
  GameState CurrentGameState();
}
