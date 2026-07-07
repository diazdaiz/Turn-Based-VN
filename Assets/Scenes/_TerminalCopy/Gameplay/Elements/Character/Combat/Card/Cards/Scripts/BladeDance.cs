using System.Collections.Generic;
using UnityEngine;
using static CardVisual;

public partial class BladeDance : Card {
    int initialCardAmount = 3;
    int upgradedCardAmount = 4;
    int CardAmount => IsUpgraded ? upgradedCardAmount : initialCardAmount;
    [SerializeField] Card shivCard;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.CreateCards(shivCard.GetType(), CardAmount, Combat.HandPile)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Add {NumberText(initialCardAmount, CardAmount)} {BrownText("Shivs")} to your hand.";
    }
}
