using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardBehavior;

public class InteractionManager : MonoBehaviour
{
    public MainScript Main;

    private CardBehaviorManager cardsManager;

    private void Start()
    {
        cardsManager = Main.CardsBehaviorManager;
    }

    private void Update()
    {
        HandleStartCardDrag();
        if(cardsManager.DraggedCard != null)
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
                cardsManager.DraggedCard.InteractionState = CardBehavior.CardInteractionState.Idle;
            }
            cardsManager.DraggedCard = null;
        }
    }

    private bool TryDrop(MapCellBehavior dropTarget)
    {
        Card card = cardsManager.DraggedCard.Model;
        bool canDrop = dropTarget.Cell.State.CanDropCardOnTile(card);
        if(canDrop)
        {
            Main.MapBehaviorManager.EnsureCellAndNeighborsExist(dropTarget.Cell.X, dropTarget.Cell.Y);

            MapCellState newState = dropTarget.Cell.State.GetFromDrop(card);
            dropTarget.Cell.State = newState;
     
            cardsManager.AddCardToTray(cardsManager.DraggedCard.Model); // For debugging
            cardsManager.DraggedCard.InteractionState = CardInteractionState.PoofingOutOfExistence;
            cardsManager.RemoveCard(cardsManager.DraggedCard);
            return true;
        }
        return false;
    }

    private MapCellBehavior GetDropTarget()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, Main.MapBehaviorManager.MapLayer))
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
            if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, Main.CardsBehaviorManager.CardsLayer))
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
