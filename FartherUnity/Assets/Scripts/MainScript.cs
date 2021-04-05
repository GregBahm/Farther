using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public bool GenerateCard;

    public int Width;
    public int Height;
    public GameObject TilePrefab;
    public ReadOnlyCollection<MapCellBehavior> MapCellBehaviors { get; private set; }
    public Worldmap WorldMap { get; private set; }
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
        WorldMap = new Worldmap(Width, Height);

        MapCellBehaviors = CreateMapCells().AsReadOnly();
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
        Tray.AddCardToTray(nextType);
        cardCreationIndex++;
        cardCreationIndex %= Enum.GetValues(typeof(CardType)).Length;
    }

    private List<MapCellBehavior> CreateMapCells()
    {
        List<MapCellBehavior> ret = new List<MapCellBehavior>();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                MapCellBehavior obj = CreateInteractionTile(x, y);
                ret.Add(obj);
            }
        }
        return ret;
    }

    private MapCellBehavior CreateInteractionTile(int x, int y)
    {
        GameObject obj = Instantiate(TilePrefab);
        obj.layer = WorldmapTransform.gameObject.layer;
        obj.transform.SetParent(WorldmapTransform);
        obj.name = x + " " + y;
        MapCellBehavior behavior = obj.GetComponent<MapCellBehavior>();
        behavior.Initialize(WorldMap[x, y]);

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
