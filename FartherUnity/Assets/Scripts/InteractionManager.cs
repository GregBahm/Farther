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
            if(dropTarget != null)
            {
                DoDrop(dropTarget);
            }
            else
            {
                cardTray.DraggedCard.State = CardBehavior.CardBehaviorState.Idle;
            }
            cardTray.DraggedCard = null;
        }
    }

    private void DoDrop(MapCellBehavior dropTarget)
    {
        UpdateMapState(dropTarget, cardTray.DraggedCard.Model);
        cardTray.DraggedCard.State = CardBehaviorState.PoofingOutOfExistence;
        cardTray.RemoveCard(cardTray.DraggedCard);
    }

    private void UpdateMapState(MapCellBehavior dropTarget, CardType dropCard)
    {
        WorldmapStateWithNeighbors droptTargetState = dropTarget.Model.GetStateWithNeighbors();
        CardDropRecipe cardDropRecipe = GetActiveCardDropRecipe(dropCard, droptTargetState);
        if(cardDropRecipe != null)
        {
            WorldmapState newState = cardDropRecipe.ModifyState(droptTargetState);
            dropTarget.Model.State = newState;
            ApplyPassiveRecipes();
        }
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
            Dictionary<WorldmapCell, WorldmapState> modifications = recipe.GetModifiedCells(MainScript.Instance.WorldMap);
            foreach (KeyValuePair<WorldmapCell, WorldmapState> entry in modifications)
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
            return hitInfo.collider.gameObject.GetComponent<MapCellBehavior>();
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
