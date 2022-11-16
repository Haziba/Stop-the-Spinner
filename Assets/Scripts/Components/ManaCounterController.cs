using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaCounterController : MonoBehaviour
{
  public GameObject Text;
  public GameObject[] Crystals;
  
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
    UpdateCrystals();
  }

  void UpdateCrystals()
  {
    for (var i = 0; i < Crystals.Length; i++)
      Crystals[i].SetActive(_mana > i);
  }

  void UpdateText()
  {
    Text.GetComponent<TextMeshProUGUI>().text = $"{_mana}/{_maxMana}";
  }
}
