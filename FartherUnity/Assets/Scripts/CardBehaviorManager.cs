using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class CardBehaviorManager : MonoBehaviour
{
    public float TrayWidth;
    public float MaxCardSpacing;

    public GameObject CardPrefab;
    public CardBehavior DraggedCard { get; set; }

    public LayerMask CardsLayer;

    public List<CardBehavior> Cards { get; } = new List<CardBehavior>();
    private readonly Dictionary<CardBehavior, Vector3> cardPositions = new Dictionary<CardBehavior, Vector3>();

    public Transform CardsTransform;

    private void UpdateTrayPositions()
    {
        float effectiveTrayWidth = GetEffectiveTrayWidth();
        for (int i = 0; i < Cards.Count; i++)
        {
            Vector3 pos = GetTrayPositionFor(i, effectiveTrayWidth);
            cardPositions[Cards[i]] = pos;
        }
    }

    private Vector3 GetTrayPositionFor(int i, float effectiveTrayWidth)
    {
        if(Cards.Count == 1)
        {
            return Vector3.zero;
        }
        float param = (float)i / (Cards.Count - 1) - .5f;
        float x = effectiveTrayWidth * param;
        return new Vector3(x, 0, 0);
    }

    internal void RemoveCard(CardBehavior draggedCard)
    {
        Cards.Remove(draggedCard);
        cardPositions.Remove(draggedCard);
        UpdateTrayPositions();
    }

    private float GetEffectiveTrayWidth()
    {
        float roomRemaining = (Cards.Count - 1) * MaxCardSpacing;
        return Mathf.Min(roomRemaining, TrayWidth);
    }

    public void AddCardToTray(Card card)
    {
        CardBehavior behavior = CreateNewCardObject(card);
        Cards.Add(behavior);
        cardPositions.Add(behavior, Vector3.zero);
        UpdateTrayPositions();
    }

    private CardBehavior CreateNewCardObject(Card card)
    {
        GameObject obj = Instantiate(CardPrefab);
        obj.layer = CardsTransform.gameObject.layer;
        obj.name = "Card " + Cards.Count;
        obj.transform.SetParent(CardsTransform);
        CardBehavior ret = obj.GetComponent<CardBehavior>();
        ret.Initialize(this, card);
        return ret;
    }

    public Vector3 GetTrayPositionFor(CardBehavior card)
    {
        return cardPositions[card];
    }
}
