using Libraries;

public class EnemyConfig : IContextObject
{
  MonsterName _name;
  public MonsterName Name => _name;

  public EnemyConfig(MonsterName name)
  {
    _name = name;
  }
}