using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapCellBehavior : MonoBehaviour
{
    public WorldmapCell Model { get; set; }
    private Material mat;

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    internal void Apply(CardType model)
    {
        Model.State.Apply(model);
        Texture2D tex = ArtBindings.Instance.GetArtFor(model).Picture;
        mat.SetTexture("_MainTex", tex);
    }
}

