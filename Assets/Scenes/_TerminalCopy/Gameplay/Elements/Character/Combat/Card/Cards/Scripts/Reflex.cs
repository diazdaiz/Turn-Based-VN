using System.Collections.Generic;
using static CardVisual;

public partial class Reflex : Card {
    int initialDrawAmount = 2;
    int upgradedDrawAmount = 3;
    int DrawAmount => IsUpgraded ? upgradedDrawAmount : initialDrawAmount;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Draws(DrawAmount)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"{BrownText("Unplayable")}.\nIf this card is discarded from your hand, draw {NumberText(initialDrawAmount, DrawAmount)} cards.";
    }
}

