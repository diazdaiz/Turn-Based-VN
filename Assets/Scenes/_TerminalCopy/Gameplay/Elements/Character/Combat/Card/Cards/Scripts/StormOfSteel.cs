using System.Collections.Generic;
using UnityEngine;

public partial class StormOfSteel : Card {
    [SerializeField] Shiv shivCard;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        Card card = new();
        card.IsUpgraded = IsUpgraded;
        List<Card> handCards = new List<Card>(Combat.HandPile);
        handCards.Remove(this);
        int amount = handCards.Count;

        return new() {
            new CombatAction.Discards(Combat.HandPile),
            new CombatAction.CreateCards(shivCard, amount, Combat.HandPile)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        string plus = IsUpgraded ? $"+" : "";
        return $"Discard your hand. Add 1 Shiv{plus} into your hand for each card discarded.";
    }
}

