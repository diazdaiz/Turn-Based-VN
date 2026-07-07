//BLOM (Nyimpen cardnya apa, costnya dijadiin 0 until played (subscribe ke play card, kalau card yang di play yang disimpen, balikin valuenya)
//pake static? jadi setup punya Dictionary<Card, SetupValue>, Setup punya static function yang subscribe OnCardPlay(balikin cost), atau OnCombatEnd(balikin semua cost)
using System.Collections.Generic;

public partial class Setup : Card {
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
        CombatAction.AfterTriggers[typeof(CombatAction.PlayCard)] += OnPlayCard;
    }

    void OnPlayCard(CombatAction combatAction) {
        CombatAction.PlayCard playCard = (CombatAction.PlayCard)combatAction;
        if (playCard == null) {
            return;
        }
        if (Cards.Contains(CombatAction.PlayCard.Card)) {
            Cards.Remove(CombatAction.PlayCard.Card);
        }
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() { new CombatAction.CardsSelection(Combat.HandPile, 1, true, After) };
    }

    void After(List<Card> cards) {
        Cards.AddRange(cards);
        Combat.DrawPile.InsertRange(0, cards);
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Place a card in your hand on top of your draw pile. It costs 0 until it is played.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 0 : 1;
    }

    private void OnDestroy() {
        CombatAction.AfterTriggers[typeof(CombatAction.PlayCard)] -= OnPlayCard;
    }
}

