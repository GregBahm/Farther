using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapCellBehavior : MonoBehaviour
{
    private bool visualsNeedUpdate;
    public MapCell Cell { get; private set; }
    private Material mat;

    private void Start()
    {
        mat = GetComponentInChildren<MeshRenderer>().material;
    }

    public void Initialize(MapCell cell)
    {
        Cell = cell;
        Cell.StateChanged += OnStateChanged;
    }

    private void Update()
    {
        if(visualsNeedUpdate)
        {
            UpdateVisuals();
            visualsNeedUpdate = false;
        }
    }

    private void OnStateChanged(object sender, EventArgs e)
    {
        visualsNeedUpdate = true;
    }

    internal void UpdateVisuals()
    {
        TileArt art = ArtBindings.Instance.GetArtFor(Cell);
        mat.SetTexture("_MainTex", art.Terrain);
    }
}