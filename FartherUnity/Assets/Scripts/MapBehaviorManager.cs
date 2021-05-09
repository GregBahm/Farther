using System;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviorManager : MonoBehaviour
{
    public MainScript Main;

    private Map map;
    public Map Map
    {
        get => map;
        set
        {
            UnbindMapEvents();
            //TODO: Delete old game objects
            this.map = value;
            BindMapEvents();
        }
    }

    public static Vector2 AscendingTileOffset { get; } = new Vector2(1, -1.73f).normalized;

    public LayerMask MapLayer;
    public Transform MapTransform;

    public GameObject MapCellPrefab;

    private readonly List<MapCellBehavior> behaviors = new List<MapCellBehavior>();

    private void UnbindMapEvents()
    {
        if(map != null)
        {
            map.CellAdded -= OnCellAdded;
        }
    }

    private void BindMapEvents()
    {
        map.CellAdded += OnCellAdded;
    }

    private void OnCellAdded(object sender, MapCell e)
    {
        CreateCellBehavior(e);
    }

    private MapCellBehavior CreateCellBehavior(MapCell cell)
    {
        GameObject obj = Instantiate(MapCellPrefab);
        obj.layer = MapTransform.gameObject.layer;
        obj.transform.SetParent(MapTransform, false);
        obj.name = cell.X + " " + cell.Y;
        MapCellBehavior behavior = obj.GetComponent<MapCellBehavior>();
        behavior.Initialize(cell);

        obj.transform.localPosition = GetCellPosition(cell.X, cell.Y);
        behaviors.Add(behavior);
        return behavior;
    }

    private Vector3 GetCellPosition(int x, int y)
    {
        Vector2 ascendingOffset = AscendingTileOffset * y;
        Vector2 offset = ascendingOffset + new Vector2(x, 0);
        return new Vector3(offset.x, offset.y, 0);
    }
}