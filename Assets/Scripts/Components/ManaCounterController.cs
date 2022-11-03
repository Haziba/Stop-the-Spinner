using UnityEngine;
using UnityEngine.UI;

public class ManaCounterController : MonoBehaviour
{
  public GameObject Text;
  
  int _maxMana;
  int _mana;

  public void Init(int maxMana)
  {
    _maxMana = maxMana;
    _mana = maxMana;

    UpdateText();
  }

  public void UpdateMana(AgentState agentState)
  {
    _mana = agentState.Mana();

    UpdateText();
  }

  void UpdateText()
  {
    Text.GetComponent<Text>().text = $"{_mana}/{_maxMana}";
  }
}
