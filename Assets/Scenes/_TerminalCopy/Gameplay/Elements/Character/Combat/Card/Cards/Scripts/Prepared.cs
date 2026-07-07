using System.Collections.Generic;
using static CardVisual;

public partial class Prepared : Card {
    int initialDrawAmount = 1;
    int upgradedDrawAmount = 2;
    int DrawAmount => IsUpgraded ? upgradedDrawAmount : initialDrawAmount;
    int initialDiscardAmount = 1;
    int upgradedDiscardAmount = 2;
    int DiscardAmount => IsUpgraded ? upgradedDiscardAmount : initialDiscardAmount;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Draws(DrawAmount),
            new CombatAction.CardsSelection(Combat.HandPile, DiscardAmount, true, (cards) => { Combat.DoAfter(new CombatAction.Discards(cards), typeof(CombatAction.CardsSelection)); })
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Draw {NumberText(initialDrawAmount, DrawAmount)} cards.\nDiscard {NumberText(initialDiscardAmount, DiscardAmount)} card.";
    }
}

