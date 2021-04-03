﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArtBindings : MonoBehaviour
{
    public static ArtBindings Instance { get; private set; }

    public CardArt[] CardArt;
    private Dictionary<CardType, CardArt> cardArtTable;

    public MapTerrainArt[] MapTerrainArt;
    private Dictionary<MapTerrainType, MapTerrainArt> terrainArtTable;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cardArtTable = CardArt.ToDictionary(item => item.Type, item => item);
        terrainArtTable = MapTerrainArt.ToDictionary(item => item.Type, item => item);
    }

    public CardArt GetArtFor(CardType cardType)
    {
        return cardArtTable[cardType];
    }
}

[Serializable]
public class MapTerrainArt
{
    public string Label;
    public MapTerrainType Type;
    public Texture2D Texture;
}

[Serializable]
public class CardArt
{
    public string Label;
    public CardType Type;
    public Texture2D Texture;
}