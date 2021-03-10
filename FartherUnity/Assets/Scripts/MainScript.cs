using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public int Width;
    public int Height;
    public GameObject TilePrefab;
    public ReadOnlyCollection<TileVisualBehavior> InteractionCells { get; private set; }
    private DesignationsGrid grid;

    public static MainScript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        grid = new DesignationsGrid(Width, Height);

        InteractionCells = CreateInteractionTiles().AsReadOnly();
    }

    private List<TileVisualBehavior> CreateInteractionTiles()
    {
        List<TileVisualBehavior> ret = new List<TileVisualBehavior>();
        Transform tiles = new GameObject("InteractionTiles").transform;
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileVisualBehavior obj = CreateInteractionTile(x, y, tiles);
                ret.Add(obj);
            }
        }
        return ret;
    }

    private TileVisualBehavior CreateInteractionTile(int x, int y, Transform tiles)
    {
        GameObject obj = Instantiate(TilePrefab);
        obj.name = x + " " + y;
        TileVisualBehavior behavior = obj.GetComponent<TileVisualBehavior>();
        behavior.Model = grid.Cells[x, y];
        obj.transform.parent = tiles;
        obj.transform.localPosition = new Vector3(x - (Width / 2), y - (Height / 2), 0);
        return behavior;
    }

    private void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(mouseRay, out hitInfo))
            {
                TileVisualBehavior cell = hitInfo.collider.gameObject.GetComponent<TileVisualBehavior>();
                //TODO: Something
            }
        }
    }
}