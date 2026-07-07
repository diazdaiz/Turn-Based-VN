using System.Collections.Generic;
using UnityEngine;

public partial class DefaultPlayerTurnCombatControl : MonoBehaviour {
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

    [SerializeField] GeneralCombatControl generalCombatControl;
    [SerializeField] Selectable EndTurnSelection;

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
    Dictionary<Card, Vector3> handPileCardsTargetPosition;
    Dictionary<Card, float> handPileCardsTargetRotation;

    public void Update() {
        if (!generalCombatControl.CurrentActionContains(typeof(CombatAction.PlayerTurn))) {
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
            float deg = 3f;
            float min = -1f / 2f * (HandPile.Count - 1) * deg / 180 * Mathf.PI;
            float adder = i * deg / 180 * Mathf.PI;
            float result = (min + adder);
            HandPileCardsTargetPosition.Add(HandPile[i], new(Mathf.Sin(result) * radius, Mathf.Cos(result) * radius - offset, 2f + 0.3f * i));
            HandPileCardsTargetRotation.Add(HandPile[i], result);
            cardsPosition.Add(HandPile[i], HandPileCardsTargetPosition[HandPile[i]]);
        }

        generalCombatControl.UpdateHoveredAndHoldedCard(cardsPosition);
        // if(HoveredCard != null) {

        // }
    }

    void TryPlayCard(Card card) {
        if (card.CanBePlayedFromHand) {
            if (Mouse.Pos.y > -2) {
                if (card.Target == Card.CardTarget.SingleEnemy) {
                    int closestIndex = -1;
                    float closestEnemyDistance = 999f;
                    for (int i = 0; i < combat.Enemies.Count; i++) {
                        float dist = (new Vector3(combat.Enemies[i].transform.position.x, combat.Enemies[i].transform.position.y, 0) - new Vector3(Mouse.Pos.x, Mouse.Pos.y, 0f)).magnitude;
                        if (dist < closestEnemyDistance) {
                            closestEnemyDistance = dist;
                            closestIndex = i;
                        }
                    }

                    if (closestEnemyDistance < 2) {
                        combat.DoOnLast(new CombatAction.PlayCard(combat.Hero, combat.Enemies[closestIndex], card), typeof(CombatAction.PlayerTurn));
                    }
                }
                else if (card.Target == Card.CardTarget.Player) {
                    combat.DoOnLast(new CombatAction.PlayCard(combat.Hero, combat.Hero, card), typeof(CombatAction.PlayerTurn));
                }
                else if (card.Target == Card.CardTarget.AllEnemies) {
                    combat.DoOnLast(new CombatAction.PlayCard(combat.Hero, new List<CharacterCombat>(combat.Enemies), card), typeof(CombatAction.PlayerTurn));
                }
            }
        }
    }

    void EndTurn() {
        if (combat.CurrentAction[0] is CombatAction.PlayerTurn PlayerTurn) {
            PlayerTurn.Finish();
        }
    }

    public void OnEnable() {
        generalCombatControl.OnReleasingHoldedCard += TryPlayCard;
        EndTurnSelection.OnSelected += EndTurn;
    }

    public void OnDisable() {
        generalCombatControl.OnReleasingHoldedCard -= TryPlayCard;
        EndTurnSelection.OnSelected -= EndTurn;
    }
}
