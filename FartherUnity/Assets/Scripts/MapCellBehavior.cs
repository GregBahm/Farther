using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapCellBehavior : MonoBehaviour
{
    private bool visualsNeedUpdate;
    public WorldmapCell Model { get; private set; }
    private Material mat;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    public void Initialize(WorldmapCell model)
    {
        Model = model;
        Model.StateChanged += OnStateChanged;
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
        // TODO: UpdateVisuals in MapCellBehavior

        //Texture2D tex = ArtBindings.Instance.GetArtFor(Model.State).Texture;
        //mat.SetTexture("_MainTex", tex);
    }
}