using UnityEngine;

public class PlayedCardController : MonoBehaviour
{
  public GameObject CardPrefab;

  CardName _cardName;
  GameObject _card;

  public void PlayCard(Agent agent, CardName cardName, GameObject previousCard)
  {
    _card = Instantiate(CardPrefab,
      transform.position,
      Quaternion.identity);
    _card.transform.parent = transform;
    _cardName = cardName;
    transform.position = new Vector3(previousCard.transform.position.x, previousCard.transform.position.y, transform.position.z);
    _card.GetComponent<CardHandler>().SetCardImage(cardName);
    if(agent != Agent.Player)
      _card.GetComponent<CardHandler>().SetAsEnemyCard();
  }

  public void RevealCard()
  {
    _card.GetComponent<CardHandler>().RevealCard();
  }

  public void RemoveCard()
  {
    Destroy(_card);
  }

  public CardName CardName()
  {
    return _cardName;
  }
}
