using System.Collections.Generic;
using UnityEngine;

public partial class CardSelectionCombatControl : MonoBehaviour {
    public Dictionary<Card, Vector3> HandPileCardsTargetPosition {
        get {
            if (handPileCardsTargetPosition == null) {
                handPileCardsTargetPosition = new();
            }
            return handPileCardsTargetPosition;
        }
        set {
            handPileCardsTargetPosition = value;
        }
    }
    public Dictionary<Card, float> HandPileCardsTargetRotation {
        get {
            if (handPileCardsTargetRotation == null) {
                handPileCardsTargetRotation = new();
            }
            return handPileCardsTargetRotation;
        }
        set {
            handPileCardsTargetRotation = value;
        }
    }
    public Dictionary<Card, Vector3> SelectionPileCardsTargetPosition {
        get {
            if (selectionPileCardsTargetPosition == null) {
                selectionPileCardsTargetPosition = new();
            }
            return selectionPileCardsTargetPosition;
        }
        set {
            selectionPileCardsTargetPosition = value;
        }
    }

    [SerializeField] GeneralCombatControl generalCombatControl;
    [SerializeField] CombatCardSelection combatCardSelection;
    [SerializeField] Selectable confirmsSelection;

    Dictionary<Card, Vector3> handPileCardsTargetPosition;
    Dictionary<Card, float> handPileCardsTargetRotation;
    Dictionary<Card, Vector3> selectionPileCardsTargetPosition;

    CombatManager combat => CombatManager.Instance;
    Card HoveredCard {
        get {
            return generalCombatControl.HoveredCard;
        }
        set {
            generalCombatControl.HoveredCard = value;
        }
    }
    Card HoldedCard {
        get {
            return generalCombatControl.HoldedCard;
        }
        set {
            generalCombatControl.HoldedCard = value;
        }
    }
    List<Card> HandPile => combat.HandPile;
    List<Card> SelectionPile => combat.SelectionPile;

    public void Update() {
        if (!generalCombatControl.CurrentActionContains(typeof(CombatAction.CardsSelection))) {
            return;
        }

        //Update hand cards target position & z rotation
        //(Untuk menentukan area klik, tapi ngefek juga untuk ke cards animation nantinya)
        Dictionary<Card, Vector3> cardsPosition = new();
        HandPileCardsTargetPosition = new();
        HandPileCardsTargetRotation = new();
        float offset = 29.5f;
        float radius = 25f;
        for (int i = 0; i < HandPile.Count; i++) {
            Card card = HandPile[i];
            float deg = 3f;
            float min = -1f / 2f * (HandPile.Count - 1) * deg / 180 * Mathf.PI;
            float adder = i * deg / 180 * Mathf.PI;
            float result = (min + adder);
            HandPileCardsTargetPosition.Add(card, new(Mathf.Sin(result) * radius, Mathf.Cos(result) * radius - offset, 2f + 0.3f * i));
            HandPileCardsTargetRotation.Add(card, result);
            cardsPosition.Add(card, HandPileCardsTargetPosition[card]);
        }

        //Update selection cards target position & z rotation
        //(Untuk menentukan area klik, tapi ngefek juga untuk ke cards animation nantinya)
        SelectionPileCardsTargetPosition = new();
        float distanceBetweenCard = 2.2f;
        for (int i = 0; i < SelectionPile.Count; i++) {
            Card card = SelectionPile[i];
            SelectionPileCardsTargetPosition.Add(card, new(distanceBetweenCard * (-(SelectionPile.Count - 1f) / 2f) + i * distanceBetweenCard, 1f, 0f));
            cardsPosition.Add(card, SelectionPileCardsTargetPosition[card]);
        }
        generalCombatControl.UpdateHoveredAndHoldedCard(cardsPosition);

        // if (HoveredCard != null && HandPile.Contains(HoveredCard)) {
        //     for (int i = 0; i < HandPile.Count; i++) {
        //         if (HoveredCard == HandPile[i]) {
        //             Debug.Log($"{i}");
        //             break;
        //         }
        //     }
        // }
    }

    void SelectCard(Card card) {
        if (combat.HandPile.Contains(card)) {
            combat.MoveCard(card, combat.SelectionPile);
        }
        else if (combat.CardSelection.Cards.Contains(card)) {
            combat.MoveCard(card, combat.HandPile);
        }
    }

    public void OnEnable() {
        generalCombatControl.OnReleasingHoldedCard += SelectCard;
        confirmsSelection.OnSelected += combatCardSelection.Confirm;
    }

    public void OnDisable() {
        generalCombatControl.OnReleasingHoldedCard -= SelectCard;
        confirmsSelection.OnSelected -= combatCardSelection.Confirm;
        HandPileCardsTargetPosition = new();
        HandPileCardsTargetRotation = new();
        SelectionPileCardsTargetPosition = new();
    }
}
