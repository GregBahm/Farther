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
    public ReadOnlyCollection<MapCellBehavior> MapCells { get; private set; }
    private Worldmap map;
    public CardsManager Tray;
    public Transform WorldmapTransform;

    public LayerMask CardsLayer;
    public LayerMask MapLayer;

    public static MainScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        map = new Worldmap(Width, Height);

        MapCells = CreateMapCells().AsReadOnly();
        CreateSomeCards();
    }

    private void CreateSomeCards()
    {
        for (int i = 0; i < 2; i++)
        {
            Tray.AddCardToTray(CardType.Fire);
        }
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
        behavior.Model = map.Cells[x, y];
        float xPos = x - ((float)Width / 2) + .5f;
        float yPos = y - ((float)Height / 2) + .5f;
        obj.transform.localPosition = new Vector3(xPos, yPos, 0);
        return behavior;
    }

    private void Update()
    {
        if(GenerateCard)
        {
            GenerateCard = false;
            Tray.AddCardToTray(CardType.Fire);
        }
    }
}
