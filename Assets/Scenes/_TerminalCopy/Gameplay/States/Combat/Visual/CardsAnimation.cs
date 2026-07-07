using System.Collections.Generic;
using UnityEngine;

//NOTE - Animasi kartu-kartu disatukan (tidak dipisah, misal animasi draw pile, animasi hand, animasi card selection, ...) 
//       karena kartu-kartu butuh animasi yang continuous dari satu tempat ke tempat lainnya
public partial class CardsAnimation : MonoBehaviour {
    [SerializeField] GeneralCombatControl generalCombatControl;
    [SerializeField] DrawPileCombatControl drawPileCombatControl;
    [SerializeField] DefaultPlayerTurnCombatControl defaultPlayerTurnCombatControl;
    [SerializeField] CardSelectionCombatControl cardSelectionCombatControl;
    [SerializeField] DiscardPileCombatControl discardPileCombatControl;
    [SerializeField] ExhaustedPileCombatControl exhaustedPileCombatControl;
    [SerializeField] GameObject drawPileContainer;
    [SerializeField] GameObject handPileContainer;
    [SerializeField] GameObject holdedCardContainer;
    [SerializeField] GameObject selectionCardsContainer;
    [SerializeField] GameObject playedPileContainer;
    [SerializeField] GameObject discardCardsContainer;
    [SerializeField] GameObject exhaustCardsContainer;
    [SerializeField] GameObject instantiatedDrawPosition;
    [SerializeField] GameObject instantiatedCreatePosition;

    CombatManager combat => CombatManager.Instance;
    Card HoveredCard => generalCombatControl.HoveredCard;
    Card HoldedCard => generalCombatControl.HoldedCard;
    List<Card> DrawPile => combat.DrawPile;
    List<Card> HandPile => combat.HandPile;
    List<Card> SelectionPile => combat.SelectionPile;
    List<Card> PlayedPile => combat.PlayedPile;
    List<Card> DiscardPile => combat.DiscardPile;
    List<Card> ExhaustedPile => combat.ExhaustedPile;

    Dictionary<Card, Vector3> targetCardsPosition;
    Dictionary<Card, float> targetCardsRotation;

    void UpdateDrawPileCardsTargetPositionAndRotation(float delta) {
        //update target position & rotation
        for (int i = 0; i < combat.DrawPile.Count; i++) {
            Card card = combat.DrawPile[i];
            targetCardsPosition.Add(card, drawPileContainer.transform.position + new Vector3(0f, 0f, 5f));
            targetCardsRotation.Add(card, 0f);
        }
    }

    void UpdateHandPileCardsTargetPositionAndRotation(float delta) {
        //update target position & rotation
        if (generalCombatControl.CurrentActionContains(typeof(CombatAction.CardsSelection))) {
            foreach (Card card in cardSelectionCombatControl.HandPileCardsTargetPosition.Keys) {
                if (card == null || !combat.CardsPileLocation.ContainsKey(card) || combat.CardsPileLocation[card] != combat.HandPile) {
                    continue;
                }
                targetCardsPosition.Add(card, cardSelectionCombatControl.HandPileCardsTargetPosition[card]);
                if (card == HoldedCard) {
                    if (card.Target == Card.CardTarget.SingleEnemy) {
                        targetCardsPosition[card] = new Vector3(Mouse.Pos.x, Mouse.Pos.y, 5.1f);
                    }
                    else if (HoldedCard.Target == Card.CardTarget.Player || HoldedCard.Target == Card.CardTarget.AllEnemies) {
                        targetCardsPosition[card] = new Vector3(Mouse.Pos.x, Mouse.Pos.y, 5.1f);
                    }
                }
                if (card == HoveredCard) {
                    targetCardsPosition[card] += new Vector3(0f, 1f, 2f);
                }
            }
            foreach (Card card in cardSelectionCombatControl.HandPileCardsTargetRotation.Keys) {
                if (combat.CardsPileLocation[card] != combat.HandPile) {
                    continue;
                }
                float targetRotation = (card == HoldedCard || card == HoveredCard) ? 0 : cardSelectionCombatControl.HandPileCardsTargetRotation[card];
                targetCardsRotation.Add(card, targetRotation);
            }
        }
        else if (generalCombatControl.CurrentActionContains(typeof(CombatAction.PlayerTurn))) {
            foreach (Card card in defaultPlayerTurnCombatControl.HandPileCardsTargetPosition.Keys) {
                targetCardsPosition.Add(card, defaultPlayerTurnCombatControl.HandPileCardsTargetPosition[card]);
                if (card == HoldedCard) {
                    if (card.Target == Card.CardTarget.SingleEnemy) {
                        targetCardsPosition[card] = new Vector3(Mouse.Pos.x, Mouse.Pos.y, 5.1f);
                    }
                    else if (HoldedCard.Target == Card.CardTarget.Player || HoldedCard.Target == Card.CardTarget.AllEnemies) {
                        targetCardsPosition[card] = new Vector3(Mouse.Pos.x, Mouse.Pos.y, 5.1f);
                    }
                }
                if (card == HoveredCard) {
                    targetCardsPosition[card] += new Vector3(0f, 1f, 2f);
                }
            }
            foreach (Card card in defaultPlayerTurnCombatControl.HandPileCardsTargetRotation.Keys) {
                float targetRotation = (card == HoldedCard || card == HoveredCard) ? 0 : defaultPlayerTurnCombatControl.HandPileCardsTargetRotation[card];
                targetCardsRotation.Add(card, targetRotation);
            }
        }
    }

    void UpdateSelectionPileCardsTargetPositionAndRotation(float delta) {
        if (!generalCombatControl.CurrentActionContains(typeof(CombatAction.CardsSelection))) {
            return;
        }

        foreach (Card card in cardSelectionCombatControl.SelectionPileCardsTargetPosition.Keys) {
            if (combat.CardsPileLocation[card] != combat.SelectionPile) {
                continue;
            }
            targetCardsPosition.Add(card, cardSelectionCombatControl.SelectionPileCardsTargetPosition[card] + new Vector3(0, 0, 15f));
            targetCardsRotation.Add(card, 0f);
        }
    }

    void UpdatePlayedPileCardsTargetPositionAndRotation(float delta) {

    }

    void UpdateDiscardPileCardsTargetPositionAndRotation(float delta) {

    }

    void UpdateExhaustedPileCardsTargetPositionAndRotation(float delta) {

    }

    void UpdateCardVisibility() {
        List<Card> allInstantiatedCard;
        allInstantiatedCard = new();
        allInstantiatedCard.AddRange(DrawPile);
        allInstantiatedCard.AddRange(HandPile);
        allInstantiatedCard.AddRange(SelectionPile);
        allInstantiatedCard.AddRange(PlayedPile);
        allInstantiatedCard.AddRange(DiscardPile);
        allInstantiatedCard.AddRange(ExhaustedPile);

        //Parent & visibility
        for (int i = 0; i < allInstantiatedCard.Count; i++) {
            Card card = allInstantiatedCard[i];
            if (DrawPile.Contains(card)) {
                card.gameObject.SetActive(false);
            }
            else if (HandPile.Contains(card)) {
                card.gameObject.SetActive(true);
            }
            else if (card != null && HoldedCard == card) {
                card.gameObject.SetActive(true);
            }
            else if (SelectionPile.Contains(card)) {
                card.gameObject.SetActive(true);
            }
            else if (PlayedPile.Contains(card)) {
                card.gameObject.SetActive(false);
            }
            else if (DiscardPile.Contains(card)) {
                card.gameObject.SetActive(false);
            }
            else if (ExhaustedPile.Contains(card)) {
                card.gameObject.SetActive(false);
            }
        }
    }

    public void Update() {
        if (combat.CurrentAction == null || combat.CurrentAction.Count == 0) {
            return;
        }
        targetCardsPosition = new();
        targetCardsRotation = new();

        //update target position & rotation based on where that card is 
        UpdateDrawPileCardsTargetPositionAndRotation(Time.deltaTime);
        UpdateHandPileCardsTargetPositionAndRotation(Time.deltaTime);
        UpdateSelectionPileCardsTargetPositionAndRotation(Time.deltaTime);
        UpdatePlayedPileCardsTargetPositionAndRotation(Time.deltaTime);
        UpdateDiscardPileCardsTargetPositionAndRotation(Time.deltaTime);
        UpdateExhaustedPileCardsTargetPositionAndRotation(Time.deltaTime);

        //update position & rotation
        foreach (Card card in targetCardsPosition.Keys) {
            card.transform.position = Vector3.Lerp(card.transform.position, targetCardsPosition[card], (targetCardsPosition[card] - card.transform.position).magnitude * 10f * Time.deltaTime);
        }
        foreach (Card card in targetCardsRotation.Keys) {
            Quaternion target = Quaternion.Euler(new(0, 0, -targetCardsRotation[card]));
            card.transform.rotation = Quaternion.Lerp(card.transform.rotation, target, (target.eulerAngles.z - card.transform.rotation.eulerAngles.z) * Time.deltaTime);
        }

        //update visibility
        UpdateCardVisibility();
    }
}
