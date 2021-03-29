using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArtBindings : MonoBehaviour
{
    public static ArtBindings Instance { get; private set; }

    public CardArt[] CardArt;
    private Dictionary<CardType, CardArt> cardArtTable;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cardArtTable = CardArt.ToDictionary(item => item.Type, item => item);
    }

    public CardArt GetArtFor(CardType cardType)
    {
        return cardArtTable[cardType];
    }
}

[Serializable]
public class CardArt
{
    public CardType Type;
    public string Name;
    public Texture2D Picture;
}