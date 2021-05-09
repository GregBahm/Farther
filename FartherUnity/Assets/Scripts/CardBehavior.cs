using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CardBehavior : MonoBehaviour
{
    private CardBehaviorManager manager;

    public Card Model { get; private set; }
    public bool IsDragging { get { return manager.DraggedCard == this; } }

    public CardInteractionState InteractionState { get; set; }

    public Vector3 HandPosition { get; set; }

    public void Initialize(CardBehaviorManager manager, Card model)
    {
        this.manager = manager;
        Model = model;
        Material mat = GetComponent<MeshRenderer>().material;
        Texture2D mainTex = ArtBindings.Instance.GetArtFor(model.Type).Texture;
        mat.SetTexture("_MainTex", mainTex);
    }

    private Vector3 dragOffset;

    private void Update()
    {
        switch (InteractionState)
        {
            case CardInteractionState.Dragging:
                DoDragUpdate();
                break;
            case CardInteractionState.PoofingOutOfExistence:
                DoPoofingOutofExistence();
                break;
            case CardInteractionState.Idle:
            default:
                DoIdleUpdate();
                break;
        }
    }

    private void DoDragUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 18));
        transform.position = mousePos + dragOffset;
    }

    private void DoPoofingOutofExistence()
    {
        Destroy(this.gameObject); // TODO: A nice animation later
    }

    private void DoIdleUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, HandPosition, Time.deltaTime * 20);
    }

    public enum CardInteractionState
    {
        Idle,
        Dragging,
        PoofingOutOfExistence, // Happens when the card has been placed on the board
    }

    internal void StartDragging()
    {
        manager.DraggedCard = this;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 18));
        dragOffset = transform.position - mousePos;
        InteractionState = CardInteractionState.Dragging;
    }
}
