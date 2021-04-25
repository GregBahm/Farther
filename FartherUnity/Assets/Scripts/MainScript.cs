using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public bool GenerateCard;

    public GameObject TilePrefab;
    private readonly List<MapCellBehavior> mapCellBehaviors = new List<MapCellBehavior>();
    public Worldmap WorldMap { get; } = new Worldmap();
    public CardsManager Tray;
    public Transform WorldmapTransform;

    public LayerMask CardsLayer;
    public LayerMask MapLayer;
    private int cardCreationIndex;

    public static MainScript Instance { get; private set; }

    public CardRecipeManager CardRecipes { get; } = new CardRecipeManager();
    public PassiveRecipeManager PassiveRecipes { get; } = new PassiveRecipeManager();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        EnsureCellAndNeighborsExist(0, 0);
        CreateSomeCards();
    }

    public void EnsureCellAndNeighborsExist(int x, int y)
    {
        CreateIfNotExistant(x, y);
        foreach (NeighborOffset offset in NeighborOffset.Offsets)
        {
            CreateIfNotExistant(x + offset.X, y + offset.Y);
        }

    }

    private void CreateIfNotExistant(int x, int y)
    {
        if(WorldMap.TryGetSlotAt(x, y) == null)
        {
            CreateInteractionTile(x, y);
        }
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
        Tray.AddCardToTray(nextType);
        cardCreationIndex++;
        cardCreationIndex %= Enum.GetValues(typeof(CardType)).Length;
    }

    private MapCellBehavior CreateInteractionTile(int x, int y)
    {
        WorldmapSlot cell = WorldMap.AddSlot(x, y);
        GameObject obj = Instantiate(TilePrefab);
        obj.layer = WorldmapTransform.gameObject.layer;
        obj.transform.SetParent(WorldmapTransform, false);
        obj.name = x + " " + y;
        MapCellBehavior behavior = obj.GetComponent<MapCellBehavior>();
        behavior.Initialize(cell);

        obj.transform.localPosition = GetCellPosition(x, y);
        return behavior;
    }


    private Vector3 GetCellPosition(int x, int y)
    {
        Vector2 ascendingOffset = Worldmap.AscendingTileOffset * y;
        Vector2 offset = ascendingOffset + new Vector2(x, 0);
        return new Vector3(offset.x, offset.y, 0);
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
