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
        dropTarget.Apply(cardTray.DraggedCard.Model);
        cardTray.DraggedCard.State = CardBehaviorState.PoofingOutOfExistence;
        cardTray.RemoveCard(cardTray.DraggedCard);
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
