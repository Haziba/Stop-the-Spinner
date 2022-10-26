using Libraries;

public class EnemyConfig : IContextObject
{
  MonsterName _name;
  public MonsterName Name => _name;
  WorldPathBackgroundName _backgroundName;
  public WorldPathBackgroundName BackgroundName => _backgroundName;

  public EnemyConfig(MonsterName name, WorldPathBackgroundName backgroundName)
  {
    _name = name;
    _backgroundName = backgroundName;
  }
}