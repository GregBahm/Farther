using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class CardBehaviorManager : MonoBehaviour
{
    private Cards cards;
    public Cards Cards 
    { 
        get => cards; 
        set
        {
            UnbindCardEvents();
            //TODO: Delete old game objects
            this.cards = value;
            BindCardEvents();
        }
    }

    public float CardHandWidth;
    public float MaxCardSpacing;

    public GameObject CardPrefab;
    public CardBehavior DraggedCard { get; set; }

    public LayerMask CardsLayer;

    private readonly List<CardBehavior> behaviors = new List<CardBehavior>();

    public Transform CardsTransform;

    private void BindCardEvents()
    {
        Cards.CardAdded += OnCardAdded;
        Cards.CardsRemoved += OnCardRemoved;
    }

    private void UnbindCardEvents()
    {
        if (Cards != null)
        {
            Cards.CardAdded -= OnCardAdded;
            Cards.CardsRemoved -= OnCardRemoved;
        }
    }

    private void OnCardRemoved(object sender, Card e)
    {
        CardBehavior behavior = behaviors.First(item => item.Model == e);
        behavior.InteractionState = CardBehavior.CardInteractionState.PoofingOutOfExistence;
        behaviors.Remove(behavior);
        UpdateCardHandPositions();
    }

    private void OnCardAdded(object sender, Card e)
    {
        CardBehavior behavior = CreateNewCardBehavior(e);
        behaviors.Add(behavior);
        UpdateCardHandPositions();
    }

    private void UpdateCardHandPositions()
    {
        float effectiveHandWidth = GetEffectiveHandWidth();
        for (int i = 0; i < behaviors.Count; i++)
        {
            Vector3 pos = GetHandPositionFor(i, effectiveHandWidth);
            behaviors[i].HandPosition = pos;
        }
    }

    private Vector3 GetHandPositionFor(int i, float effectiveTrayWidth)
    {
        if (behaviors.Count == 1)
        {
            return Vector3.zero;
        }
        float param = (float)i / (behaviors.Count - 1) - .5f;
        float x = effectiveTrayWidth * param;
        return new Vector3(x, 0, 0);
    }

    private float GetEffectiveHandWidth()
    {
        float roomRemaining = (behaviors.Count - 1) * MaxCardSpacing;
        return Mathf.Min(roomRemaining, CardHandWidth);
    }

    private CardBehavior CreateNewCardBehavior(Card card)
    {
        GameObject obj = Instantiate(CardPrefab);
        obj.layer = CardsTransform.gameObject.layer;
        obj.name = card.Type.ToString();
        obj.transform.SetParent(CardsTransform);
        CardBehavior ret = obj.GetComponent<CardBehavior>();
        ret.Initialize(this, card);
        return ret;
    }
}
