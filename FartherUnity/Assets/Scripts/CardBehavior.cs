using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CardBehavior : MonoBehaviour
{
    private CardsManager tray;

    public CardType Model { get; private set; }
    public bool IsDragging { get { return tray.DraggedCard == this; } }

    public CardBehaviorState State { get; set; }

    public void Initialize(CardsManager tray, CardType model)
    {
        this.tray = tray;
        Model = model;
    }

    private Vector3 dragOffset;

    private void Update()
    {
        switch (State)
        {
            case CardBehaviorState.Dragging:
                DoDragUpdate();
                break;
            case CardBehaviorState.PoofingOutOfExistence:
                DoPoofingOutofExistence();
                break;
            case CardBehaviorState.Idle:
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
        Vector3 positionTarget = tray.GetTrayPositionFor(this);
        transform.localPosition = Vector3.Lerp(transform.localPosition, positionTarget, Time.deltaTime * 20);
    }

    public enum CardBehaviorState
    {
        Idle,
        Dragging,
        PoofingOutOfExistence, // Happens when the card has been placed on the board
    }

    internal void StartDragging()
    {
        tray.DraggedCard = this;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 18));
        dragOffset = transform.position - mousePos;
        State = CardBehaviorState.Dragging;
    }
}
