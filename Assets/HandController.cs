using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Libraries;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    public GameObject CardPrefab;
    public bool PlayerHand;
    public GameObject DrawCounter;
    public GameObject DiscardCounter;

    IList<HandCard> _handCards;
    IList<CardName> _drawPile;
    IList<CardName> _discardPile;
    CardName[] _deck;
    int _maxCardsInHand;

    public event EventHandler OnCardClicked;

    public HandController() {
      _handCards = new List<HandCard>();

      _drawPile = new List<CardName>();
      _discardPile = new List<CardName>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetDeck(CardName[] deck, int maxCardsInHand)
    {
      _deck = deck;
      _maxCardsInHand = maxCardsInHand;

      Shuffle();
      DealHand();
    }

    void Shuffle()
    {
      _drawPile = new List<CardName>(_deck);
      foreach(var handCard in _handCards)
        _drawPile.RemoveAt(_drawPile.IndexOf(handCard.CardName()));
      _drawPile.Shuffle();

      _discardPile = new List<CardName>();

      UpdateCounters();
    }

    void DealHand()
    {
      if(_handCards.Count == 0)
        Shuffle();

      for(var i = 0; i < _maxCardsInHand - 1; i++)
        DrawCard();
    }

    public void DrawCard()
    {
      if (!_drawPile.Any())
        if (_discardPile.Any())
          Shuffle();
        else
          return;

      if (_handCards.Count >= _maxCardsInHand)
        return;
      
      AddCard(_drawPile.First());
      _drawPile.RemoveAt(0);
      UpdateCounters();
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

    public void DiscardCard(CardName cardName)
    {
      _discardPile.Add(cardName);
      UpdateCounters();
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

    public void UpdateCounters()
    {
      DrawCounter.GetComponent<Text>().text = _drawPile.Count.ToString();
      DiscardCounter.GetComponent<Text>().text = _discardPile.Count.ToString();
    }

    // todo: Maybe agentState should live elsewhere
    public int[] AvailableCardIndexes(AgentState agentState)
    {
      if (!_handCards.Any())
        return Array.Empty<int>();
      
      return Enumerable.Range(0, _handCards.Count - 1).Where(i => agentState.CanPlayCard(_handCards[i].CardName()))
        .ToArray();
    }
}
