public class EnemyConfig : IContextObject
{
  EnemyName _name;
  public EnemyName Name => _name;

  public EnemyConfig(EnemyName name)
  {
    _name = name;
  }
}