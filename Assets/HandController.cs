using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject CardPrefab;
    public bool PlayerHand;

    IList<HandCard> _handCards;
    CardName[] _deck;

    public event EventHandler OnCardClicked;

    public HandController() {
      _handCards = new List<HandCard>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetDeck(CardName[] deck)
    {
      _deck = deck;

      DealHand();
    }

    void DealHand()
    {
      _deck.Shuffle();
      for(var i = 0; i < 5; i++)
        AddCard(_deck[i]);
    }

    void AddCard(CardName cardName)
    {
      // todo: This whole thing is janky, does HandCard need to exist?
      var card = Instantiate(CardPrefab, transform.position, Quaternion.identity);
      var handCard = new HandCard(cardName, card, gameObject);
      if(!PlayerHand)
        card.GetComponent<CardHandler>().SetAsEnemyCard();
      handCard.OnCardClicked += CardClicked;
      _handCards.Add(handCard);

      RefreshCardPositions();
    }
    
    public void RemoveCard(HandCard handCard)
    {
      Destroy(handCard.Image());
      _handCards.Remove(handCard);
      RefreshCardPositions();
    }

    void CardClicked(object sender, EventArgs e)
    {
      OnCardClicked?.Invoke(this, e);
    }

    void RefreshCardPositions()
    {
      var curveOffsetCount = (5f - _handCards.Count) / 2f;

      for(var i = 0; i < _handCards.Count; i++) {
        var handCard = _handCards[i];

        var curveHeight = Mathf.Cos((((float)(i+curveOffsetCount) / 2f) - 1f));
        if(!PlayerHand)
          curveHeight *= -1;
        /*Debug.Log(i);
        Debug.Log(curveHeight);*/

        handCard.Image().transform.position = transform.position + new Vector3(i * 0.9f - 0.45f * _handCards.Count, (float)curveHeight, -0.1f*i);
        //handCard.Image().transform.eulerAngles.z = (-2.5f + curvedOffsetCount * 350f % 360);
      }
    }

    public int TotalCards()
    {
      return _handCards.Count;
    }

    public HandCard CardAt(int index)
    {
      return _handCards[index];
    }
}
