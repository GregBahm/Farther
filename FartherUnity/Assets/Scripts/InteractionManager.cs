using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardBehavior;

public class InteractionManager : MonoBehaviour
{
    public MainScript Main;

    private CardsVisualManager cardTray;

    private void Start()
    {
        cardTray = MainScript.Instance.CardsVisualManager;
    }

    private void Update()
    {
        HandleStartCardDrag();
        if(cardTray.DraggedCard != null)
        {
            HandleCardDrag();
        }
    }

    private void HandleCardDrag()
    {
        if(!Input.GetMouseButton(0))
        {
            MapCellBehavior dropTarget = GetDropTarget();
            bool dropped = false;
            if(dropTarget != null)
            {
                dropped = TryDrop(dropTarget);
            }
            if(!dropped)
            {
                cardTray.DraggedCard.State = CardBehavior.CardBehaviorState.Idle;
            }
            cardTray.DraggedCard = null;
        }
    }

    private bool TryDrop(MapCellBehavior dropTarget)
    {
        Card card = new Card(cardTray.DraggedCard.Model);// TODO: update the model to be a card
        bool canDrop = dropTarget.Model.State.CanDropCardOnTile(card);
        if(canDrop)
        {
            Main.WorldmapVisualManager.EnsureCellAndNeighborsExist(dropTarget.Model.X, dropTarget.Model.Y);

            WorldmapState newState = dropTarget.Model.State.GetFromDrop(card);
            dropTarget.Model.State = newState;
     
            cardTray.AddCardToTray(cardTray.DraggedCard.Model); // For debugging
            cardTray.DraggedCard.State = CardBehaviorState.PoofingOutOfExistence;
            cardTray.RemoveCard(cardTray.DraggedCard);
            return true;
        }
        return false;
    }

    private MapCellBehavior GetDropTarget()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, Main.WorldmapVisualManager.MapLayer))
        {
            return hitInfo.collider.transform.parent.gameObject.GetComponent<MapCellBehavior>();
        }
        return null;
    }

    private void HandleStartCardDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, Main.CardsVisualManager.CardsLayer))
            {
                CardBehavior card = hitInfo.collider.gameObject.GetComponent<CardBehavior>();
                if (card != null)
                {
                    card.StartDragging();
                }
            }
        }
    }
}
