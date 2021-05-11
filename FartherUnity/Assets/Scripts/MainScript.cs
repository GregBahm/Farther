using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public CardBehaviorManager CardsBehaviorManager;
    public MapBehaviorManager MapBehaviorManager;

    public Game Game { get; private set; }

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
        CreateInitialHand();
    }

    private void CreateInitialHand()
    {
        Game.Cards.Add(new Card(CardType.Earth));
        Game.Cards.Add(new Card(CardType.Water));
        Game.Cards.Add(new Card(CardType.Plants));
        Game.Cards.Add(new WarriorCard());
    }
}
