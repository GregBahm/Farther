using System;
using System.Collections.Generic;

public class Cards
{
    public event EventHandler<Card> CardAdded;
    public event EventHandler<Card> CardsRemoved;

    private readonly HashSet<Card> hand = new HashSet<Card>();
    public IEnumerable<Card> Hand { get; }

    public void Remove(Card card)
    {
        hand.Remove(card);
        CardsRemoved?.Invoke(this, card);
    }

    public void Add(Card card)
    {
        hand.Add(card);
        CardAdded?.Invoke(this, card);
    }
    public void Add(IEnumerable<Card> cards)
    {
        foreach (Card card in cards)
        {
            Add(card);
        }
    }
}