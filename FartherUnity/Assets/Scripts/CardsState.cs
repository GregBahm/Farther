using System;
using System.Collections.Generic;

public class CardsState
{
    public event EventHandler<IEnumerable<Card>> CardsAdded;
    public event EventHandler<Card> CardsRemoved;

    private readonly HashSet<Card> hand = new HashSet<Card>();
    public IEnumerable<Card> Hand { get; }

    public void Remove(Card card)
    {
        hand.Remove(card);
        CardsRemoved?.Invoke(this, card);
    }

    public void Add(IEnumerable<Card> cards)
    {
        foreach (Card card in cards)
        {
            hand.Add(card);
        }
        CardsAdded?.Invoke(this, cards);
    }
}