using System.Linq;
using Libraries;
using TMPro;
using UnityEngine;

public class ToolTipController : MonoBehaviour
{
  public GameObject Details;

  bool _preparedToBeHidden;

  public void Start()
  {
    gameObject.SetActive(false);
  }

  public void Update()
  {
    if (!gameObject.activeSelf)
      return;

    if (!Input.touches.Any())
      _preparedToBeHidden = true;
    if (_preparedToBeHidden && Input.touches.Any())
      HideToolTip();
  }
    
  public void ShowToolTip(CardName cardName)
  {
    gameObject.SetActive(true);
    Details.GetComponent<TextMeshProUGUI>().text = CardLibrary.Cards[cardName].ToolTip.Text();
  }

  void HideToolTip()
  {
    gameObject.SetActive(false);
  }
}