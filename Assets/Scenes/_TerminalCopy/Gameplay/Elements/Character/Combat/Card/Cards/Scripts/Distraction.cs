using System.Collections.Generic;
using static CardVisual;

public partial class Distraction : Card {
    public static List<Card> Cards {
        get {
            if (cards == null) {
                cards = new List<Card>();
            }
            return cards;
        }
        set {
            cards = value;
        }
    }
    public static List<Card> cards;

    private void Awake() {
        Cards = new List<Card>();
        CombatAction.AfterTriggers[typeof(CombatAction.CreateCard)] += OnCreateCard;
        CombatAction.AfterTriggers[typeof(CombatAction.PlayerTurn)] += OnEndPlayerTurn;
    }

    void OnCreateCard(CombatAction combatAction) {
        CombatAction.CreateCard createCard = (CombatAction.CreateCard)combatAction;
        if (createCard == null) {
            return;
        }
        Cards.Add(createCard.InstantiatedCard);
    }

    void OnEndPlayerTurn(CombatAction combatAction) {
        Cards.Clear();
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        Card card = Card.GetRandom(CardIdentity.Silent, false, true, false);
        return new() {
            new CombatAction.CreateCard(card, Combat.HandPile)
        };
    }

    private void OnDestroy() {
        CombatAction.AfterTriggers[typeof(CombatAction.CreateCard)] -= OnCreateCard;
        CombatAction.AfterTriggers[typeof(CombatAction.PlayerTurn)] -= OnEndPlayerTurn;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Add a random Skill to your hand. It costs 0 this turn.\n{BrownText("Exhaust")}.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 0 : 1;
    }
}

