using Libraries;

public class EnemyConfig : IContextObject
{
  MonsterName _name;
  public MonsterName Name => _name;
  BackgroundName _backgroundName;
  public BackgroundName BackgroundName => _backgroundName;

  public EnemyConfig(MonsterName name, BackgroundName backgroundName)
  {
    _name = name;
    _backgroundName = backgroundName;
  }
}