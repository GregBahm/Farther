using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public bool GenerateCard;

    public Game Game { get; private set; }

    public CardBehaviorManager CardsBehaviorManager;
    public MapBehaviorManager MapBehaviorManager;

    private int cardCreationIndex;

    public static MainScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Game = new Game();
        CardsBehaviorManager.Cards = Game.Cards;
        MapBehaviorManager.Map = Game.Map;
        Game.Map.EnsureCellAndNeighborsExist(0, 0);
        CreateSomeCards();
    }

    private void CreateSomeCards()
    {
        for (int i = 0; i < 5; i++)
        {
            CreateCard();
        }
    }

    private void CreateCard()
    {
        CardType nextType = (CardType)cardCreationIndex;
        Game.Cards.Add(new Card(nextType));
        cardCreationIndex++;
        cardCreationIndex %= Enum.GetValues(typeof(CardType)).Length;
    }

    private void Update()
    {
        if(GenerateCard)
        {
            GenerateCard = false;
            CreateCard();
        }
    }
}
