using UnityEngine;

public class DamageBallController : MonoBehaviour
{
  public void SetImage(CardName cardName)
  {
    HideAllImages();
    ShowImage(cardName);
  }
  
  void HideAllImages()
  {
    for(var i = 0; i < transform.childCount; i++){
      transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowImage(CardName cardName)
  {
    var card = transform.Find(cardName.ToString());
    if(card == null)
      transform.Find("Default").gameObject.SetActive(true);
    else
      card.gameObject.SetActive(true);;
  }
}
