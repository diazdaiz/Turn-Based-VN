using System.Collections.Generic;
using static CardVisual;

public partial class Acrobatics : Card {
    int initialDrawAmount = 3;
    int upgradedDrawAmount = 4;
    int drawAmount => IsUpgraded ? upgradedDrawAmount : initialDrawAmount;
    int discardAmount => 1;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Draws(drawAmount),
            new CombatAction.CardsSelection(Combat.HandPile, discardAmount, true, (cards) => { Combat.DoAfter(new CombatAction.Discards(cards), typeof(CombatAction.CardsSelection)); })
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Draw {NumberText(initialDrawAmount, drawAmount)} cards.\nDiscard 1 card.";
    }
}
