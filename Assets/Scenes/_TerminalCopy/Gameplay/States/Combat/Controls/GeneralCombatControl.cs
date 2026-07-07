using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GeneralCombatControl : MonoBehaviour {
    public Card HoveredCard { get; set; }
    public Card HoldedCard { get; set; }
    public Action<Card> OnReleasingHoldedCard;

    [SerializeField] GameObject drawPileContainer;
    [SerializeField] GameObject handPileContainer;
    [SerializeField] GameObject holdedCardContainer;
    [SerializeField] GameObject selectionCardsContainer;
    [SerializeField] GameObject playedPileContainer;
    [SerializeField] GameObject discardCardsContainer;
    [SerializeField] GameObject exhaustCardsContainer;

    CombatManager combat => CombatManager.Instance;
    List<Card> DrawPile => combat.DrawPile;
    List<Card> HandPile => combat.HandPile;
    List<Card> SelectionPile => combat.SelectionPile;
    List<Card> PlayedPile => combat.PlayedPile;
    List<Card> DiscardPile => combat.DiscardPile;
    List<Card> ExhaustedPile => combat.ExhaustedPile;
    List<Card> allInstantiatedCard;
    Card HoveredCardSelectionCandidate;
    Card HoveredCardHandCandidate;

    public bool CurrentActionContains(Type type) {
        if (combat.CurrentAction == null || combat.CurrentAction.Count == 0) {
            return false;
        }
        for (int i = combat.CurrentAction.Count - 1; i >= 0; i--) {
            if (combat.CurrentAction[i].GetType() == type) {
                return true;
            }
        }
        return false;
    }

    public void UpdateHoveredAndHoldedCard(Dictionary<Card, Vector3> cardsPosition) {
        //Update hovered card
        if (HoldedCard == null) {
            Card closestCard = null;
            float closestCardDistance = 999f; //closest ke target hand placement
            foreach (Card card in cardsPosition.Keys) {
                float dist = new Vector2(cardsPosition[card].x - Mouse.Pos.x, cardsPosition[card].y - Mouse.Pos.y).magnitude;
                if (dist < closestCardDistance) {
                    closestCardDistance = dist;
                    closestCard = card;
                }
            }

            if (closestCardDistance < 2) {
                HoveredCard = closestCard;
            }
            else {
                HoveredCard = null;
            }
        }

        //Update holded card
        if (Mouse.IsJustPressed(Mouse.Key.Left)) {
            if (HoveredCard != null) {
                HoldedCard = HoveredCard;
                HoveredCard = null;
            }
        }
        if (Mouse.IsJustReleased(Mouse.Key.Left)) {
            if (HoldedCard != null) {
                OnReleasingHoldedCard?.Invoke(HoldedCard);
                HoldedCard = null;
            }
        }
    }

    void UpdateCardParent() {
        //Parent & visibility
        for (int i = 0; i < allInstantiatedCard.Count; i++) {
            Card card = allInstantiatedCard[i];
            if (DrawPile.Contains(card)) {
                card.transform.parent = drawPileContainer.transform;
            }
            else if (HandPile.Contains(card)) {
                card.transform.parent = handPileContainer.transform;
            }
            else if (card != null && HoldedCard == card) {
                card.transform.parent = holdedCardContainer.transform;
            }
            else if (combat.CardSelection.Cards.Contains(card)) {
                card.transform.parent = selectionCardsContainer.transform;
            }
            else if (combat.PlayedPile.Contains(card)) {
                card.transform.parent = playedPileContainer.transform;
            }
            else if (DiscardPile.Contains(card)) {
                card.transform.parent = discardCardsContainer.transform;
            }
            else if (ExhaustedPile.Contains(card)) {
                card.transform.parent = exhaustCardsContainer.transform;
            }
        }
    }

    public void Update() {
        if (combat.CurrentAction == null || combat.CurrentAction.Count == 0) {
            return;
        }
        allInstantiatedCard = new();
        allInstantiatedCard.AddRange(DrawPile);
        allInstantiatedCard.AddRange(HandPile);
        allInstantiatedCard.AddRange(SelectionPile);
        allInstantiatedCard.AddRange(PlayedPile);
        allInstantiatedCard.AddRange(DiscardPile);
        allInstantiatedCard.AddRange(ExhaustedPile);
        UpdateCardParent();
    }
}
