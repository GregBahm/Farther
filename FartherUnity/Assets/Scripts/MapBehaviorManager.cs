using UnityEngine;

public class MapBehaviorManager : MonoBehaviour
{
    public MainScript Main;

    public static Vector2 AscendingTileOffset { get; } = new Vector2(1, -1.73f).normalized;

    public LayerMask MapLayer;
    public Transform MapTransform;

    public GameObject MapCellPrefab;

    private MapCellBehavior CreateInteractionTile(int x, int y)
    {
        MapCellPosition cell = Main.Game.Map.AddPosition(x, y);
        GameObject obj = Instantiate(MapCellPrefab);
        obj.layer = MapTransform.gameObject.layer;
        obj.transform.SetParent(MapTransform, false);
        obj.name = x + " " + y;
        MapCellBehavior behavior = obj.GetComponent<MapCellBehavior>();
        behavior.Initialize(cell);

        obj.transform.localPosition = GetCellPosition(x, y);
        return behavior;
    }

    private Vector3 GetCellPosition(int x, int y)
    {
        Vector2 ascendingOffset = AscendingTileOffset * y;
        Vector2 offset = ascendingOffset + new Vector2(x, 0);
        return new Vector3(offset.x, offset.y, 0);
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
        if (Main.Game.Map.TryGetPositionAt(x, y) == null)
        {
            CreateInteractionTile(x, y);
        }
    }
}