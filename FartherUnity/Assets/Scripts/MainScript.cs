using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public bool GenerateCard;

    public GameState CurrentState { get; private set; }

    public CardsVisualManager CardsVisualManager;
    public WorldmapVisualManager WorldmapVisualManager;

    private int cardCreationIndex;

    public static MainScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CurrentState = new GameState();
        WorldmapVisualManager.EnsureCellAndNeighborsExist(0, 0);
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
        CardsVisualManager.AddCardToTray(nextType);
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
