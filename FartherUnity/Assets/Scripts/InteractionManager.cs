using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CardBehavior;

public class InteractionManager : MonoBehaviour
{
    private CardsManager cardTray;

    private void Start()
    {
        cardTray = MainScript.Instance.Tray;
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
        WorldmapStateWithNeighbors dropTargetState = dropTarget.Model.GetStateWithNeighbors();
        CardDropRecipe cardDropRecipe = GetActiveCardDropRecipe(cardTray.DraggedCard.Model, dropTargetState);
        if(cardDropRecipe != null)
        {
            MainScript.Instance.EnsureCellAndNeighborsExist(dropTarget.Model.X, dropTarget.Model.Y);
            UpdateMapState(dropTarget, dropTargetState, cardDropRecipe);

            cardTray.AddCardToTray(cardTray.DraggedCard.Model); // For debugging
            cardTray.DraggedCard.State = CardBehaviorState.PoofingOutOfExistence;
            cardTray.RemoveCard(cardTray.DraggedCard);
            return true;
        }
        return false;
    }

    private void UpdateMapState(MapCellBehavior dropTarget, WorldmapStateWithNeighbors dropTargetState, CardDropRecipe cardDropRecipe)
    {
        WorldmapState newState = cardDropRecipe.ModifyState(dropTargetState);
        dropTarget.Model.State = newState;
        //ApplyPassiveRecipes();
    }

    private CardDropRecipe GetActiveCardDropRecipe(CardType dropCard, WorldmapStateWithNeighbors droptTargetState)
    {
        return MainScript.Instance.CardRecipes
            .Where(item => item.Card == dropCard)
            .FirstOrDefault(item => item.CanModifyState(droptTargetState));
    }

    private void ApplyPassiveRecipes()
    {
        foreach (PassiveRecipe recipe in MainScript.Instance.PassiveRecipes)
        {
            Dictionary<WorldmapSlot, WorldmapState> modifications = recipe.GetModifiedCells(MainScript.Instance.WorldMap);
            foreach (KeyValuePair<WorldmapSlot, WorldmapState> entry in modifications)
            {
                entry.Key.State = entry.Value;
            }
        }
    }

    private MapCellBehavior GetDropTarget()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, MainScript.Instance.MapLayer))
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
            if (Physics.Raycast(mouseRay, out hitInfo, float.MaxValue, MainScript.Instance.CardsLayer))
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
